﻿using System;
using System.Linq;
using System.Text;

namespace CardValidator
{
    class Program
    {
        public const int VisaNumberLow = 4000;
        public const int VisaNumberHigh = 4999;
        public const int AExpressNumberLow1 = 3400;
        public const int AExpressNumberHigh1 = 3499;
        public const int AExpressNumberLow2 = 3700;
        public const int AExpressNumberHigh2 = 3799;
        public const int MaestroNumberLow1 = 5000;
        public const int MaestroNumberHigh1 = 5099;
        public const int MaestroNumberLow2 = 5600;
        public const int MaestroNumberHigh2 = 6999;
        public const int MCardNumberLow1 = 2221;
        public const int MCardNumberHigh1 = 2720;
        public const int MCardNumberLow2 = 5100;
        public const int MCardNumberHigh2 = 5599;
        public const int JcbNumberLow = 3528;
        public const int JcbNumberHigh = 3589;

        enum Vendor
        {
            AmericanExpress,
            Maestro,
            MasterCard,
            Visa,
            Jcb,
            Unknown
        }

        static Vendor GetCreditCardVendor(string number)
        {
            int numberConverted = Convert.ToInt32(number.Substring(0, 4));

            if (numberConverted >= VisaNumberLow & numberConverted <= VisaNumberHigh)
            {
                return Vendor.Visa;
            }

            else if ((numberConverted >= AExpressNumberLow1 & numberConverted <= AExpressNumberHigh1) ||
                (numberConverted >= AExpressNumberLow2 & numberConverted <= AExpressNumberHigh2))
            {
                return Vendor.AmericanExpress;
            }

            else if ((numberConverted >= MaestroNumberLow1 & numberConverted <= MaestroNumberHigh1) ||
                (numberConverted >= MaestroNumberLow2 & numberConverted <= MaestroNumberHigh2))
            {
                return Vendor.Maestro;
            }

            else if ((numberConverted >= MCardNumberLow1 & numberConverted <= MCardNumberHigh1) ||
                (numberConverted >= MCardNumberLow2 & numberConverted <= MCardNumberHigh2))
            {
                return Vendor.MasterCard;
            }

            else if (numberConverted >= JcbNumberLow & numberConverted <= JcbNumberHigh)
            {
                return Vendor.Jcb;
            }

            return Vendor.Unknown;
        }

        static bool IsCreditCardNumberValid(string number)
        {
            return GetLuhnSumOfDigits(number) % 10 == 0;
        }

        static string GenerateNextCreditCardNumber(string number)
        {
            StringBuilder incNumber = new StringBuilder(IncrementNumber(number));
            incNumber[incNumber.Length - 1] = '0';
            int luhnSum = GetLuhnSumOfDigits(incNumber.ToString());
            int num = (luhnSum / 10 == 0) ? 0 : 10 - luhnSum % 10;
            incNumber[incNumber.Length - 1] = Convert.ToChar('0' + num);
            return incNumber.ToString();
        }

        static string IncrementNumber(string number)
        {
            var array = number.Select(v => Convert.ToInt32(v) - '0').ToArray();
            int sum = array[array.Length - 2] + 1;

            int i = array.Length - 2;

            while (sum > 9 && i > 0)
            {
                array[i] = sum - 10;
                sum = array[i + 1] + 1;
                i--;
            }

            array[i] = (sum < 10) ? sum : sum - 10;

            var incrementedNumber = array.Aggregate(string.Empty, (s, v) => s + v.ToString());

            return (sum > 10 && i == 0) ? "1" + incrementedNumber : incrementedNumber;
        }

        static int GetLuhnSumOfDigits(string number)
        {
            return number.Reverse().
                Select((v, i) => new { value = Convert.ToInt32(v) - '0', index = i }).
                 Sum(g => (g.index % 2 == 0) ? g.value : GetSumOfDigits(g.value * 2));
        }

        static int GetSumOfDigits(int number)
        {
            int sum = 0;
            int buf = number;

            do
            {
                while (buf > 0)
                {
                    sum += buf % 10;
                    buf = buf / 10;
                }

                buf = sum;
            }
            while (buf > 9);

            return sum;
        }

        static void Main(string[] args)
        {
            string clinedNumber = "";
            string cardNumber;

            while (clinedNumber.Length < 12)
            {
                Console.WriteLine("Enter card number:");
                cardNumber = Console.ReadLine();
                clinedNumber = cardNumber.Replace(" ", "");

                if (!clinedNumber.All(char.IsDigit))
                {
                    Console.WriteLine("Card number contain wrong symbols.\nPlease enter card number:");
                    clinedNumber = "";
                }
            }



            Console.WriteLine(GetCreditCardVendor(clinedNumber).ToString());

            string isNumberValid = IsCreditCardNumberValid(clinedNumber) ? "valid" : "not valid";
            Console.WriteLine("Card number is " + isNumberValid);

            string nextNumber = GenerateNextCreditCardNumber(clinedNumber);
            Console.WriteLine("Next card number is " + nextNumber);

            isNumberValid = IsCreditCardNumberValid(nextNumber) ? "valid" : "not valid";
            Console.WriteLine("Next card number is " + isNumberValid);

            Console.ReadLine();
        }
    }
}