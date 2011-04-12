namespace Bingo.Web.Models
{
    public class Ball
    {
        public char Letter { get; set; }

        public int Number { get; set; }

        public override bool Equals(object obj)
        {
            var that = obj as Ball;

            if (that == null)
                return false;

            return Letter == that.Letter && Number == that.Number;
        }

        public override int GetHashCode()
        {
            return Letter.GetHashCode() + 27 * Number.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Letter, Number);
        }
    }
}