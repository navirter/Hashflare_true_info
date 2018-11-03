using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using My;
using System.Threading;
using System.Xml.Serialization;
using System.IO;

namespace Hashflare_true_info
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string infoPath = Directory.GetCurrentDirectory() + "\\info.xml";
        public Info info;
        private void digitOrControl(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && !Char.IsPunctuation(e.KeyChar))
                e.Handled = true;
        }
        void reshowInfo()
        {
            try
            {
                #region sha-256
                tbSHAunitPrice.Text = info.sha256.unitPrice.ToString();
                tbSHAhashrate.Text = info.sha256.hashrate10GHS.ToString();
                tbSHAelectricity.Text = info.sha256.electricity.ToString();
                tbSHAincome.Text = info.sha256.income.ToString();
                tbSHAcleanPerDay.Text = info.sha256.cleanperday.ToString();
                tbSHAinvested.Text = info.sha256.invested.ToString();
                tbSHAadditionalInfo.Text = info.sha256.additionalInfo;
                #endregion

                #region scrypt
                tbSCRYPTunitPrice.Text = info.scrypt.unitPrice.ToString();
                tbSCRYPThashrate.Text = info.scrypt.hashrate1MHS.ToString();
                tbSCRYPTelectricity.Text = info.scrypt.electricity.ToString();
                tbSCRYPTincome.Text = info.scrypt.income.ToString();
                tbSCRYPTcleanPerDay.Text = info.scrypt.cleanperday.ToString();
                tbSCRYPTinvested.Text = info.scrypt.invested.ToString();
                tbSCRYPTadditionalInfo.Text = info.scrypt.additionalInfo;
                #endregion

                #region ethsash
                tbETHASHunitPrice.Text = info.ethash.unitPrice.ToString();
                tbETHASHhashrate.Text = info.ethash.hashrate1MHS.ToString();
                tbETHASHelectricity.Text = info.ethash.electricity.ToString();
                tbETHASHincome.Text = info.ethash.income.ToString();
                tbETHASHcleanPerDay.Text = info.ethash.cleanperday.ToString();
                tbETHASHinvested.Text = info.ethash.invested.ToString();
                tbETHASHadditionalInfo.Text = info.ethash.additionalInfo;
                #endregion

                tb_general_additional_info.Text = info.general.additionalInfo;
                tb_general_taken_out.Text = info.general.takenOutDollars.ToString();
            }
            catch { }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                info = ThreadSeeker.ReadFromXmlFile<Info>(infoPath);
                reshowInfo();
            }
            catch (Exception ej)
            { info = new Info(); }
        }

        int getDoublementTime(double capital, double profitability)
        {
            if (capital <= 0 || profitability <= 0)
                return 0;
            double profit = 0;
            double target = capital;
            int days = 0;
            while (profit < target)
            {
                double part = capital * profitability / 100;
                if (part <= 0) break;
                capital += part;
                profit += part;
                days++;
            }
            return days;
        }

        double getElectricityProcent(double dailyIncome, double electricityCost)
        {
            var electricityProcent = electricityCost * 100 / dailyIncome;
            return electricityProcent;
        }

        #region sha-256

        private void tbSHAunitPrice_TextChanged(object sender, EventArgs e)
        {
            shagetTotalValue();
            tbSHAincome_TextChanged(sender, e);
            process_general_info();
            ThreadSeeker.WriteToXmlFile(infoPath, info);
        }
        private void tbSHAhashrate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                info.sha256.hashrate10GHS = double.Parse(tbSHAhashrate.Text.Trim().Replace(".", ","));//10 ghs
                info.sha256.electricity = info.sha256.hashrate10GHS * 0.0035;
                tbSHAelectricity.Text = info.sha256.electricity.ToString();
                shagetTotalValue();
                shagetPer10GHS();
                tbSHAincome_TextChanged(sender, e);
                process_general_info();
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch (Exception eu) { }
        }
        void shagetPer10GHS()
        {
            try
            {
                info.sha256.cleanperdayfor10ghs = info.sha256.cleanperday / info.sha256.hashrate10GHS;
                tbSHAper10GHS.Text = info.sha256.cleanperdayfor10ghs.ToString();
                shagetProfitability();
            }
            catch { }
        }
        void shagetTotalValue()
        {
            try
            {
                info.sha256.unitPrice = double.Parse(tbSHAunitPrice.Text.Trim().Replace(".", ","));
                info.sha256.hashratePrice = info.sha256.hashrate10GHS * info.sha256.unitPrice;
                tbSHAprice.Text = info.sha256.hashratePrice.ToString();
            }
            catch { }
        }

        private void tbSHAincome_TextChanged(object sender, EventArgs e)
        {
            try
            {
                info.sha256.income = double.Parse(tbSHAincome.Text.Trim().Replace(".", ","));
                info.sha256.cleanperday = info.sha256.income - info.sha256.electricity;
                tbSHAcleanPerDay.Text = info.sha256.cleanperday.ToString();
                info.sha256.cleanperdayProcent = info.sha256.cleanperday * 100 / info.sha256.hashratePrice;
                tbSHAdayprocent.Text = info.sha256.cleanperdayProcent.ToString();
                info.sha256.cleanperdayProcentOfInvested = info.sha256.cleanperday * 100 / info.sha256.invested;
                tbSHAdayprocentOfInvested.Text = info.sha256.cleanperdayProcentOfInvested.ToString();
                info.sha256.cleanPerMonth = info.sha256.cleanperday * 30;
                tbSHAcleanPerMonth.Text = info.sha256.cleanPerMonth.ToString();

                info.sha256.electricityProcent = getElectricityProcent(info.sha256.income, info.sha256.electricity);
                tbSHAelectricityProcent.Text = info.sha256.electricityProcent.ToString();

                shagetPer10GHS();
                process_general_info();
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }

        private void tbSHAprice_TextChanged(object sender, EventArgs e)
        {
            shagetProfitability();
            info.sha256.electricityProcent = getElectricityProcent(info.sha256.income, info.sha256.electricity);
            tbSHAelectricityProcent.Text = info.sha256.electricityProcent.ToString();
            process_general_info();
            ThreadSeeker.WriteToXmlFile(infoPath, info);
        }
        void shagetProfitability()
        {
            try
            {
                info.sha256.profitability = info.sha256.hashratePrice * 100 / info.sha256.invested - 100;
                tbSHAprofitability.Text = info.sha256.profitability.ToString();
                info.sha256.capitalPayback = 0;
                double schonadded = 0;
                if (info.sha256.cleanperday > 0)
                    while (schonadded < info.sha256.hashratePrice) { schonadded += info.sha256.cleanperday; info.sha256.capitalPayback++; }
                tbSHApayback_of_capital.Text = info.sha256.capitalPayback.ToString();
                info.sha256.investedPayback = 0;
                schonadded = 0;
                if (info.sha256.cleanperday > 0)
                    while (schonadded < info.sha256.invested) { schonadded += info.sha256.cleanperday; info.sha256.investedPayback++; }
                tbSHApayback_of_invested.Text = info.sha256.investedPayback.ToString();
                info.sha256.doublement = getDoublementTime(info.sha256.hashratePrice, info.sha256.cleanperdayProcent);
                tbSHAdoublement.Text = info.sha256.doublement.ToString();
                info.sha256.invdoublement = getDoublementTime(info.sha256.invested, info.sha256.cleanperdayProcentOfInvested);
                tbSHAinvdoublement.Text = info.sha256.invdoublement.ToString();
            }
            catch (Exception eh)
            { }
        }


        private void tbSHAinvested_TextChanged(object sender, EventArgs e)
        {
            try
            {
                info.sha256.invested = double.Parse(tbSHAinvested.Text.Trim().Replace(".", ","));
                info.sha256.cleanperdayProcentOfInvested = info.sha256.cleanperday * 100 / info.sha256.invested;
                tbSHAdayprocentOfInvested.Text = info.sha256.cleanperdayProcentOfInvested.ToString();
                shagetProfitability();
                process_general_info();
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }

        private void tbSHAadditionalInfo_TextChanged(object sender, EventArgs e)
        {
            info.sha256.additionalInfo = tbSHAadditionalInfo.Text;
            try
            {
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }
        #endregion

        #region scrypt

        private void tbSCRYPTunitPrice_TextChanged(object sender, EventArgs e)
        {
            scryptgetTotalValue();
            tbSCRYPTincome_TextChanged(sender, e);
            process_general_info();
            ThreadSeeker.WriteToXmlFile(infoPath, info);
        }
        private void tbSCRYPThashrate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                info.scrypt.hashrate1MHS = double.Parse(tbSCRYPThashrate.Text.Trim().Replace(".", ","));//10 ghs
                info.scrypt.electricity = info.scrypt.hashrate1MHS * 0.005;
                tbSCRYPTelectricity.Text = info.scrypt.electricity.ToString();
                scryptgetTotalValue();
                scryptgetPer1MHS();
                tbSCRYPTincome_TextChanged(sender, e);
                process_general_info();
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch (Exception eu) { }
        }
        void scryptgetPer1MHS()
        {
            try
            {
                info.scrypt.cleanperdayfor1mhs = info.scrypt.cleanperday / info.scrypt.hashrate1MHS;
                tbSCRYPTPer1MHS.Text = info.scrypt.cleanperdayfor1mhs.ToString();
                scryptgetProfitability();
            }
            catch { }
        }
        void scryptgetTotalValue()
        {
            try
            {
                info.scrypt.unitPrice = double.Parse(tbSCRYPTunitPrice.Text.Trim().Replace(".", ","));
                info.scrypt.hashratePrice = info.scrypt.hashrate1MHS * info.scrypt.unitPrice;
                tbSCRYPTprice.Text = info.scrypt.hashratePrice.ToString();
            }
            catch { }
        }

        private void tbSCRYPTincome_TextChanged(object sender, EventArgs e)
        {
            try
            {
                info.scrypt.income = double.Parse(tbSCRYPTincome.Text.Trim().Replace(".", ","));
                info.scrypt.cleanperday = info.scrypt.income - info.scrypt.electricity;
                tbSCRYPTcleanPerDay.Text = info.scrypt.cleanperday.ToString();
                info.scrypt.cleanperdayProcent = info.scrypt.cleanperday * 100 / info.scrypt.hashratePrice;
                tbSCRYPTdayProcent.Text = info.scrypt.cleanperdayProcent.ToString();
                info.scrypt.cleanperdayProcentOfInvested = info.scrypt.cleanperday * 100 / info.scrypt.invested;
                tbSCRYPTdayProcentOfInvested.Text = info.scrypt.cleanperdayProcentOfInvested.ToString();
                info.scrypt.cleanPerMonth = info.scrypt.cleanperday * 30;
                tbSCRYPTcleanPerMonth.Text = info.scrypt.cleanPerMonth.ToString();
                scryptgetPer1MHS();

                info.scrypt.electricityProcent = getElectricityProcent(info.scrypt.income, info.scrypt.electricity);
                tbSCRYPTelectricityProcent.Text = info.scrypt.electricityProcent.ToString();

                process_general_info();
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }

        private void tbSCRYPTprice_TextChanged(object sender, EventArgs e)
        {
            scryptgetProfitability();
            process_general_info();

            info.scrypt.electricityProcent = getElectricityProcent(info.scrypt.income, info.scrypt.electricity);
            tbSCRYPTelectricityProcent.Text = info.scrypt.electricityProcent.ToString();

            ThreadSeeker.WriteToXmlFile(infoPath, info);
        }
        void scryptgetProfitability()
        {
            try
            {
                info.scrypt.profitability = info.scrypt.hashratePrice * 100 / info.scrypt.invested - 100;
                tbSCRYPTprofitability.Text = info.scrypt.profitability.ToString();
                info.scrypt.capitalPayback = 0;
                double schonadded = 0;
                if (info.scrypt.cleanperday > 0)
                    while (schonadded < info.scrypt.hashratePrice) { schonadded += info.scrypt.cleanperday; info.scrypt.capitalPayback++; }
                tbSCRYPTpayback_of_capital.Text = info.scrypt.capitalPayback.ToString();
                info.scrypt.investedPayback = 0;
                schonadded = 0;
                if (info.scrypt.cleanperday > 0)
                    while (schonadded < info.scrypt.invested) { schonadded += info.scrypt.cleanperday; info.scrypt.investedPayback++; }
                tbSCRYPTpayback_of_invested.Text = info.scrypt.investedPayback.ToString();
                info.scrypt.doublement = getDoublementTime(info.scrypt.hashratePrice, info.scrypt.cleanperdayProcent);
                tbSCRYPTdoublement.Text = info.scrypt.doublement.ToString();
                info.scrypt.invdoublement = getDoublementTime(info.scrypt.invested, info.scrypt.cleanperdayProcentOfInvested);
                tbSCRYPTinvdoublement.Text = info.scrypt.invdoublement.ToString();
            }
            catch (Exception eh)
            { }
        }

        private void tbSCRYPTinvested_TextChanged(object sender, EventArgs e)
        {
            try
            {
                info.scrypt.invested = double.Parse(tbSCRYPTinvested.Text.Trim().Replace(".", ","));
                info.scrypt.cleanperdayProcentOfInvested = info.scrypt.cleanperday * 100 / info.scrypt.invested;
                tbSCRYPTdayProcentOfInvested.Text = info.scrypt.cleanperdayProcentOfInvested.ToString();
                scryptgetProfitability();
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }

        private void tbSCRYPTadditionalInfo_TextChanged(object sender, EventArgs e)
        {
            info.scrypt.additionalInfo = tbSCRYPTadditionalInfo.Text;
            try
            {
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }
        #endregion

        #region ethash

        private void tbETHASHunitPrice_TextChanged(object sender, EventArgs e)
        {
            ethashgetTotalValue();
            tbETHASHincome_TextChanged(sender, e);
            process_general_info();
            ThreadSeeker.WriteToXmlFile(infoPath, info);
        }
        private void tbETHASHhashrate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                info.ethash.hashrate1MHS = double.Parse(tbETHASHhashrate.Text.Trim().Replace(".", ","));//10 ghs
                info.ethash.electricity = info.ethash.hashrate1MHS * 0;
                tbETHASHelectricity.Text = info.ethash.electricity.ToString();
                ethashgetTotalValue();
                ethashgetPer1MHS();
                tbETHASHincome_TextChanged(sender, e);
                process_general_info();
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch (Exception eu) { }
        }
        void ethashgetPer1MHS()
        {
            try
            {
                info.ethash.cleanperdayfor1mhs = info.ethash.cleanperday / info.ethash.hashrate1MHS;
                tbETHASHper1MHS.Text = info.ethash.cleanperdayfor1mhs.ToString();
                ethashgetProfitability();
            }
            catch { }
        }
        void ethashgetTotalValue()
        {
            try
            {
                info.ethash.unitPrice = double.Parse(tbETHASHunitPrice.Text.Trim().Replace(".", ","));
                info.ethash.hashratePrice = info.ethash.hashrate1MHS * info.ethash.unitPrice;
                tbETHASHprice.Text = info.ethash.hashratePrice.ToString();
            }
            catch { }
        }

        private void tbETHASHincome_TextChanged(object sender, EventArgs e)
        {
            try
            {
                info.ethash.income = double.Parse(tbETHASHincome.Text.Trim().Replace(".", ","));
                info.ethash.cleanperday = info.ethash.income - info.ethash.electricity;
                tbETHASHcleanPerDay.Text = info.ethash.cleanperday.ToString();
                info.ethash.cleanperdayProcent = info.ethash.cleanperday * 100 / info.ethash.hashratePrice;
                tbETHASHdayProcent.Text = info.ethash.cleanperdayProcent.ToString();
                info.ethash.cleanperdayProcentOfInvested = info.ethash.cleanperday * 100 / info.ethash.invested;
                tbETHASHdayProcentOfInvested.Text = info.ethash.cleanperdayProcentOfInvested.ToString();
                info.ethash.cleanPerMonth = info.ethash.cleanperday * 30;
                tbETHASHcleanPerMonth.Text = info.ethash.cleanPerMonth.ToString();
                ethashgetPer1MHS();

                info.ethash.electricityProcent = getElectricityProcent(info.ethash.income, info.ethash.electricity);
                tbETHASHelectricityProcent.Text = info.ethash.electricityProcent.ToString();

                process_general_info();
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }

        private void tbETHASHprice_TextChanged(object sender, EventArgs e)
        {
            ethashgetProfitability();
            process_general_info();

            info.ethash.electricityProcent = getElectricityProcent(info.ethash.income, info.ethash.electricity);
            tbETHASHelectricityProcent.Text = info.ethash.electricityProcent.ToString();

            ThreadSeeker.WriteToXmlFile(infoPath, info);
        }
        void ethashgetProfitability()
        {
            try
            {
                info.ethash.profitability = info.ethash.hashratePrice * 100 / info.ethash.invested - 100;
                tbETHASHprofitability.Text = info.ethash.profitability.ToString();
                info.ethash.capitalPayback = 0;
                double schonadded = 0;
                if (info.ethash.cleanperday > 0)
                    while (schonadded < info.ethash.hashratePrice) { schonadded += info.ethash.cleanperday; info.ethash.capitalPayback++; }
                tbETHASHpayback_of_capital.Text = info.ethash.capitalPayback.ToString();
                info.ethash.investedPayback = 0;
                schonadded = 0;
                if (info.ethash.cleanperday > 0)
                    while (schonadded < info.ethash.invested) { schonadded += info.ethash.cleanperday; info.ethash.investedPayback++; }
                tbETHASHpayback_of_invested.Text = info.ethash.investedPayback.ToString();
                info.ethash.doublement = getDoublementTime(info.ethash.hashratePrice, info.ethash.cleanperdayProcent);
                tbETHASHdoublement.Text = info.ethash.doublement.ToString();
                info.ethash.invdoublement = getDoublementTime(info.ethash.invested, info.ethash.cleanperdayProcentOfInvested);
                tbETHASHinvdoublement.Text = info.ethash.invdoublement.ToString();
            }
            catch (Exception eh)
            { }
        }

        private void tbETHASHinvested_TextChanged(object sender, EventArgs e)
        {
            try
            {
                info.ethash.invested = double.Parse(tbETHASHinvested.Text.Trim().Replace(".", ","));
                info.ethash.cleanperdayProcentOfInvested = info.ethash.cleanperday * 100 / info.ethash.invested;
                tbETHASHdayProcentOfInvested.Text = info.ethash.cleanperdayProcentOfInvested.ToString();
                ethashgetProfitability();
                process_general_info();
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }

        private void tbETHASHadditionalInfo_TextChanged(object sender, EventArgs e)
        {
            info.ethash.additionalInfo = tbETHASHadditionalInfo.Text;
            try
            {
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }
        #endregion

        #region general
        void process_general_info()
        {
            renew_general_info();
            reshow_general_info();
        }
        void renew_general_info()
        {
            string last = ""; try { last = info.general.additionalInfo; } catch { }
            info.general = new Info.General() { additionalInfo = last };
            var i = info;
            var g = info.general;
            try
            {
                g.price = i.sha256.hashratePrice + i.scrypt.hashratePrice + i.ethash.hashratePrice;
                g.invested = i.sha256.invested + i.scrypt.invested + i.ethash.invested;
                g.electricity = i.sha256.electricity + i.scrypt.electricity + i.ethash.electricity;
                g.income = i.sha256.income + i.scrypt.income + i.ethash.income;
                g.dollar_daily = g.income - g.electricity;
                g.dollar_monthly = g.dollar_daily * 30;
            }
            catch (Exception e)
            { }
            try
            {
                g.profit_percent_of_price_daily = g.dollar_daily * 100 / g.price;
                g.profit_percent_of_invested_daily = g.dollar_daily * 100 / g.invested;
                g.electricity_percent_of_income = g.electricity * 100 / g.income;
            }
            catch (Exception e)
            { }
            try
            {
                g.price_doublement_for_days = getDoublementTime(g.price, g.profit_percent_of_price_daily);
                g.invested_doublement_for_days = getDoublementTime(g.invested, g.profit_percent_of_invested_daily);
                info.general.capitalPayback = 0;
                double schonadded = 0;
                if (info.general.dollar_daily > 0)
                    while (schonadded < info.general.price) { schonadded += info.general.dollar_daily; info.general.capitalPayback++; }
                info.general.investedPayback = 0;
                schonadded = 0;
                if (info.general.dollar_daily > 0)
                    while (schonadded < info.general.invested) { schonadded += info.general.dollar_daily; info.general.investedPayback++; }
                g.profitability = g.price * 100 / g.invested - 100;
            }
            catch (Exception e)
            { }
        }
        void reshow_general_info()
        {
            /*public double price = 0;
                            public double invested = 0;
                            public double electricity = 0;
                            public double income = 0;
                            public double dollar_daily = 0;

                            public double profit_percent_of_price_daily = 0;
                            public double profit_percent_of_invested_daily = 0;
                            public double electricity_percent_of_income = 0;

                            public int price_doublement_for_days = 0;
                            public int payback_for_days = 0;
                            public int invested_doublement_for_days = 0;*/
            var g = info.general;
            try
            {
                tb_general_price.Text = g.price.ToString();
                tb_general_invested.Text = g.invested.ToString();
                tb_general_electricity.Text = g.electricity.ToString();
                tb_general_income.Text = g.income.ToString();
                tb_general_dollar_per_day.Text = g.dollar_daily.ToString();
                tb_general_dollar_per_month.Text = g.dollar_monthly.ToString();
            }
            catch { }
            try
            {
                tb_general_profit_of_price_daily.Text = g.profit_percent_of_price_daily.ToString();
                tb_general_profit_of_invested_daily.Text = g.profit_percent_of_invested_daily.ToString();
                tb_general_electricity_of_income.Text = g.electricity_percent_of_income.ToString();
            } catch { }
            try
            {
                tb_general_doublement_of_price.Text = g.price_doublement_for_days.ToString();
                tb_general_doublement_of_invested.Text = g.invested_doublement_for_days.ToString();
                tb_general_return_of_capital.Text = g.capitalPayback.ToString();
                tb_general_return_of_invested.Text = g.investedPayback.ToString();
                tb_general_profitability.Text = g.profitability.ToString();
            }
            catch { }
        }

        private void tb_general_additional_info_TextChanged(object sender, EventArgs e)
        {
            info.general.additionalInfo = tb_general_additional_info.Text;
            try
            {
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }

        private void tb_general_taken_out_TextChanged(object sender, EventArgs e)
        {
            try
            {
                info.general.takenOutDollars = int.Parse(tb_general_taken_out.Text);
                ThreadSeeker.WriteToXmlFile(infoPath, info);
            }
            catch { }
        }
        #endregion
    }
}
