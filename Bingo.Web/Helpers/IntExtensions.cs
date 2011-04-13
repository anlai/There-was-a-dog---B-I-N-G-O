namespace Bingo.Web.Helpers
{
    public static class IntExtensions
    {
        public static string ToBingoBall(this int num)
        {
            var character = "O";

            if (num <= 15)
            {
                character = "B";
            }
            else if (num <= 30)
            {
                character = "I";
            }
            else if (num <= 45)
            {
                character = "N";
            }
            else if (num <= 60)
            {
                character = "G";
            }

            return character + num;
        }
    }
}