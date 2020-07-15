//BY drudiverse
//FROM: https://answers.unity.com/questions/37756/how-to-turn-a-string-to-an-int.html

namespace OpenMMO
{
    public partial class Tools
    {
        public static int ParseFast(string value)
        {
            int result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char letter = value[i];
                result = 10 * result + (letter - 48);
            }
            return result;
        }
    }
}