using System.Globalization;

namespace DTO
{
    public class FormatCurrency
    {
        private static readonly string[] Units = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
        private static readonly string[] Tens = { "", "mười", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };

        public static string NumberToWords(decimal number)
        {
            if (number == 0) return "không đồng";
            if (number < 0) return "âm " + NumberToWords(Math.Abs(number));

            string words = "";

            // Chia số thành phần tỷ, triệu, nghìn và đơn vị
            long billions = (long)(number / 1_000_000_000);
            number %= 1_000_000_000;

            long millions = (long)(number / 1_000_000);
            number %= 1_000_000;

            long thousands = (long)(number / 1_000);
            number %= 1_000;

            long units = (long)number;

            // Xử lý phần tỷ
            if (billions > 0)
            {
                words += ConvertGroup(billions) + " tỷ ";
            }

            // Xử lý phần triệu
            if (millions > 0)
            {
                words += ConvertGroup(millions) + " triệu ";
            }

            // Xử lý phần nghìn
            if (thousands > 0)
            {
                words += ConvertGroup(thousands) + " nghìn ";
            }

            // Xử lý phần đơn vị
            if (units > 0)
            {
                words += ConvertGroup(units);
            }

            return words.Trim() + " đồng";
        }

        private static string ConvertGroup(long number)
        {
            string groupWords = "";

            long hundreds = number / 100;
            number %= 100;

            long tens = number / 10;
            long units = number % 10;

            // Hàng trăm
            if (hundreds > 0)
            {
                groupWords += Units[hundreds] + " trăm ";
            }
            else if (groupWords != "")
            {
                groupWords += "không trăm ";
            }

            // Hàng chục và đơn vị
            if (tens > 0)
            {
                groupWords += Tens[tens] + " ";
                if (units > 0)
                {
                    groupWords += (tens == 1 && units == 5 ? "lăm" : Units[units]) + " ";
                }
            }
            else if (units > 0)
            {
                groupWords += (units == 5 ? "lăm" : "lẻ " + Units[units]) + " ";
            }

            return groupWords.Trim();
        }

        public static string formatCurrency(decimal amount)
        { // Chuyển đổi thành chuỗi với định dạng "80.000"
            string formatted = amount.ToString("#,##0", CultureInfo.InvariantCulture); // Thêm đơn vị "đ"
            return formatted + "đ";
        }
    }
}
