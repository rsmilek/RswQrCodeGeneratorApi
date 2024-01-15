using QRCoder;
using System;
using System.Numerics;

namespace RswQrCodeGeneratorApi.Domain.Payloads
{
    public class CzPayment : PayloadGenerator.Payload
    {
        private const string Br = "*";

        private string _prefix;
        private string _account;
        private string _bank;
        private string _amount;
        private string _variableSymbol;
        private string _specificSymbol;
        private string _constantSymbol;
        private string _message;

        private readonly Dictionary<char, string> CountryCharToNr = new Dictionary<char, string>()
        {
            {'C', "12"},
            {'Z', "35"}
        };

        public CzPayment(string prefix, string account, string bank, string amount,
            string variableSymbol, string specificSymbol, string constantSymbol, string message)
        {
            _prefix = prefix;
            _account = account;
            _bank = bank;
            _amount = amount;
            _variableSymbol = variableSymbol;
            _specificSymbol = specificSymbol;
            _constantSymbol = constantSymbol;
            _message = message;
        }

        /// <summary>
        //return $"ACC:CZ5855000000001265098001 AM:480.55 CC:CZK X-VS:1234567890 X-SS:1234567890 X-KS:1234567890";
        //return "SPD*1.0*ACC:CZ4755000000001071001005*AM:11662*CC:CZK*X-KS:3559*X-VS:4419089933";
        //return $"SPD*1.0*ACC:CZ5855000000001265098001*AM:480.55*CC:CZK*X-VS:1234567890*X-SS:1234567890*X-KS:1234567890";
        //return $"SPD*1.0*ACC:CZ5830300000001234567890*AM:123.00*CC:CZK*X-VS:12*X-SS:34*X-KS:56";
        //return "SPD*1.0*ACC:CZ3230300000001234567890*AM:123.00*CC:CZK*";
        /// </summary>
        public override string ToString()
        {
            string result = $"SPD*1.0{Br}";

            if (!string.IsNullOrEmpty(_prefix) || !string.IsNullOrEmpty(_account) || !string.IsNullOrEmpty(_bank))
            {
                var czIBAN = CalcCzIBANCheckDigit(_prefix, _account, _bank);
                result += $"ACC:CZ{czIBAN.checkDigit}{czIBAN.bban}{Br}";
            }

            if (!string.IsNullOrEmpty(_amount))
                result += $"AM:{_amount}{Br}CC:CZK{Br}";

            if (!string.IsNullOrEmpty(_variableSymbol))
                result += $"X-VS:{_variableSymbol}{Br}";

            if (!string.IsNullOrEmpty(_specificSymbol))
                result += $"X-SS:{_specificSymbol}{Br}";

            if (!string.IsNullOrEmpty(_constantSymbol))
                result += $"X-KS:{_constantSymbol}{Br}";

            if (!string.IsNullOrEmpty(_message))
                result += $"MSG:{_message}{Br}";

            return result;
        }

        private (string checkDigit, string bban) CalcCzIBANCheckDigit(string prefix, string accout, string bank)
        {
            prefix = prefix.PadLeft(6, '0');
            accout = accout.PadLeft(10, '0');
            bank = bank.PadLeft(4, '0');

            var bban = $"{bank}{prefix}{accout}";
            var ibanChangeover = $"{bban}{CountryCharToNr['C']}{CountryCharToNr['Z']}00";

            // Calculate modulo 97
            var highNumber = BigInteger.Parse(ibanChangeover);
            var modulo97 = (int)(highNumber % 97);

            // Calculate the check digit
            int checkDigit = 98 - modulo97;

            // Format the check digit to always be two digits
            return (checkDigit.ToString("00"), bban);
        }
    }
}
