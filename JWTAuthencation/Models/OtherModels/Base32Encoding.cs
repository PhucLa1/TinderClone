namespace JWTAuthencation.Models.OtherModels
{
    public static class Base32Encoding
    {
        private const string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static byte[] ToBytes(string input)
        {
            input = input.Trim().Replace(" ", "").Replace("-", "").ToUpper();
            int numBytes = input.Length * 5 / 8;
            byte[] bytes = new byte[numBytes];

            int byteIndex = 0, bitIndex = 0;
            int inputIndex = 0;
            byte workingByte = 0;
            while (byteIndex < numBytes)
            {
                int val = Base32Chars.IndexOf(input[inputIndex]);
                if (val >= 0)
                {
                    workingByte = (byte)(workingByte << 5 | val);
                    bitIndex += 5;
                    if (bitIndex >= 8)
                    {
                        bytes[byteIndex] = (byte)(workingByte >> (bitIndex - 8));
                        bitIndex -= 8;
                        byteIndex++;
                    }
                }
                inputIndex++;
            }

            return bytes;
        }
    }
}
