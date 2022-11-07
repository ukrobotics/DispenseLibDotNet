/*
MIT License

Copyright (c) 2021 UK ROBOTICS

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

____________________________________________________________________________

For support - please contact us at  www.ukrobotics.com
 */

using System;
using System.Collections.Generic;
using System.Text;
using UKRobotics.Common;
using UKRobotics.Common.Xml;

namespace UKRobotics.D2.DispenseLib.Common
{
    /// <summary>
    /// SBS well address in format A1 - LetterNumber as defined by SBS format, ie 96,384,1536... etc
    /// </summary>
    public class SBSWellAddress
    {
        public static readonly XmlAttributeConst SBSWellAddressAttrConst = new XmlAttributeConst("sbs-address");

        public static readonly int AlphabetCount = Constants.ZUpperChar - Constants.AUpperChar + 1;// ie 26

        private string m_letters;
        private int m_number;


        /// <summary>
        /// copy ctor
        /// </summary>
        /// <param name="sbsWellAddress"></param>
        public SBSWellAddress(SBSWellAddress sbsWellAddress)
        {
            m_letters = sbsWellAddress.m_letters;
            m_number = sbsWellAddress.m_number;
        }

        public SBSWellAddress(string letters, int number)
        {
            m_letters = letters.ToUpper();
            m_number = number;
        }

        public SBSWellAddress(char letter, int number)
            : this(letter.ToString(), number)
        {
        }

        /// <summary>
        /// create by parsing string of format A1
        /// </summary>
        /// <param name="addressString"></param>
        public SBSWellAddress(string addressString)
        {
            FromString(addressString);
        }

        public static SBSWellAddress Parse(string addressString)
        {
            return new SBSWellAddress(addressString);
        }

        /// <summary>
        /// Parses a list of wells  eg A1,A2,A3 .
        /// Compliments method <see cref="ToStringList"/>
        /// </summary>
        /// <param name="wellList"></param>
        /// <returns></returns>
        public static List<SBSWellAddress> ParseList(string wellList)
        {
            try
            {
                wellList = wellList.Trim();

                string[] strings = wellList.Split(Constants.CommaChar);

                List<SBSWellAddress> wells = new List<SBSWellAddress>();

                foreach (string s in strings)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        string wellsString = s.Trim(Constants.WhiteSpaceChar, Constants.TabChar);

                        if (wellsString.Contains("-"))
                        {
                            string[] fromTo = wellsString.Split('-');
                            SBSWellAddress fromWell = Parse(fromTo[0].Trim(Constants.WhiteSpaceChar, Constants.TabChar));
                            SBSWellAddress toWell = Parse(fromTo[1].Trim(Constants.WhiteSpaceChar, Constants.TabChar));

                            for (int letterIndex = fromWell.LettersIndex; letterIndex <= toWell.LettersIndex; letterIndex++)
                            {
                                for (int numberIndex = fromWell.NumberIndex; numberIndex <= toWell.NumberIndex; numberIndex++)
                                {
                                    SBSWellAddress well = new SBSWellAddress(GetLettersForIndex(letterIndex), numberIndex + 1);
                                    wells.Add(well);
                                }
                            }
                        }
                        else
                        {
                            SBSWellAddress well = Parse(wellsString);
                            wells.Add(well);
                        }

                    }
                }

                return wells;
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Error while parsing well address list: [{0}]", wellList), e);
            }
        }

        public void FromString(string addressString)
        {
            addressString = addressString.Trim();

            StringBuilder letters = new StringBuilder();
            StringBuilder numbers = new StringBuilder();

            int stringLength = addressString.Length;
            for (int i = 0; i < stringLength; i++)
            {
                char c = addressString[i];
                if (char.IsLetter(c))
                {
                    letters.Append(c);
                }
                else
                {
                    numbers.Append(addressString.Substring(i));
                    break;
                }
            }

            if (letters.Length < 1)
            {
                throw new ArgumentException(
                    string.Format("Invalid format, failed to parse {0}",
                                addressString));
            }

            m_letters = letters.ToString().ToUpper();

            string numbersString = numbers.ToString();
            try
            {
                m_number = int.Parse(numbersString);
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    string.Format("Invalid format, failed to parse numbers {0} of string {1}", numbersString,
                                  addressString), e);
            }
        }

        public string Letters
        {
            get { return m_letters; }
        }

        public int Number
        {
            get { return m_number; }
        }

        public int Row
        {
            get
            {
                int row, column;
                GetRowAndColumn(m_letters, m_number, out row, out column);
                return row;
            }
        }

        public int Column
        {
            get
            {
                int row, column;
                GetRowAndColumn(m_letters, m_number, out row, out column);
                return column;
            }
        }

        /// <summary>
        /// return the letter ( eg A, B, C, ... Z, AA ) etc as an index starting with A==0
        /// </summary>
        public int LettersIndex
        {
            get
            {
                return GetIndexForLetters(Letters);
            }
        }

        /// <summary>
        /// Get the letters for a given position index.
        /// </summary>
        /// <param name="index">0 - N  where 0 == A, 1 == B etc</param>
        /// <returns></returns>
        private static string GetLettersForIndex(int index)
        {
            if (index < 0)
            {
                throw new ArgumentException(string.Format("Invalid index for well letter {0}", index));
            }

            // Create letters, starting at A and going to AA, AB, AC etc
            StringBuilder letterBuffer = new StringBuilder();
            double alphaBase = AlphabetCount;
            double currentValue = index;
            int letterCount;
            if (0 != currentValue)
            {
                letterCount = (int)Math.Floor(Math.Log(currentValue, alphaBase)) + 1;
            }
            else
            {
                letterCount = 1;
            }
            for (int letterIndex = letterCount; letterIndex > 0; letterIndex--)
            {
                int multiplierBase = (int)Math.Pow(alphaBase, letterIndex - 1);
                int multiplierValue = (int)(currentValue / multiplierBase);
                letterBuffer.Append(ConvertToBaseAlphaChar(multiplierValue, letterIndex));
                currentValue = currentValue % multiplierBase;
            }

            return letterBuffer.ToString();
        }

        private static int GetIndexForLetters(string letters)
        {
            double alphaBase = AlphabetCount;

            int index = 0;
            int lettersCount = letters.Length;
            for (int letterIndex = lettersCount; letterIndex > 0; letterIndex--)
            {
                int multiplierBase = (int)Math.Pow(alphaBase, letterIndex - 1);
                char c = letters[(lettersCount - letterIndex)];// INDEX IN REVERSE ORDER
                int baseAlpha = ConvertFromBaseAlphaChar(c, letterIndex);
                index += (baseAlpha * multiplierBase);
            }

            return index;
        }

        /// <summary>
        /// return index similar to <see cref="LettersIndex"/>, for the number. EG Number-1  so that Number1 == 0
        /// </summary>
        public int NumberIndex
        {
            get { return Number - 1; }
        }

        /// <summary>
        /// By convention ( UK Robotics ) maps A,B,C into X axis
        /// </summary>
        public int XIndex
        {
            get { return Row; }
        }

        /// <summary>
        /// By convention ( UK Robotics ) maps 1,2,3 ( ie number after the letter ) into Y axis
        /// </summary>
        public int YIndex
        {
            get { return Column; }
        }



        /// <summary>
        /// convert row,column to A1 format. Note that we should have more numbers than letters,
        /// ie A1-H12 would be 8*12 plate. We map row to letter and column to number
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static SBSWellAddress MatrixToSBSWellAddress(int row, int column)
        {
            string letter;
            int number;
            GetLetterAndNumber(row, column, out letter, out number);

            return new SBSWellAddress(letter, number);
        }

        /// <summary>
        /// Convert row/column index (0-n) to letter/numbers, ie A1 and upwards
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="letters"></param>
        /// <param name="number"></param>
        public static void GetLetterAndNumber(int row, int column, out string letters, out int number)
        {
            if (row < 0)
            {
                throw new ArgumentOutOfRangeException("row", row, "Invalid row value");
            }
            if (column < 0)
            {
                throw new ArgumentOutOfRangeException("column", column, "Invalid column value");
            }


            // Create letters, starting at A and going to AA, AB, AC etc
            StringBuilder letterBuffer = new StringBuilder();
            double alphaBase = AlphabetCount;
            double currentValue = row;
            int letterCount;
            if (0 != currentValue)
            {
                letterCount = (int)Math.Floor(Math.Log(currentValue, alphaBase)) + 1;
            }
            else
            {
                letterCount = 1;
            }
            for (int letterIndex = letterCount; letterIndex > 0; letterIndex--)
            {
                int multiplierBase = (int)Math.Pow(alphaBase, letterIndex - 1);
                int multiplierValue = (int)(currentValue / multiplierBase);
                letterBuffer.Append(ConvertToBaseAlphaChar(multiplierValue, letterIndex));
                currentValue = currentValue % multiplierBase;
            }

            letters = letterBuffer.ToString();

            number = column + 1;
        }

        public static void GetRowAndColumn(string letters, int number, out int row, out int column)
        {
            double alphaBase = AlphabetCount;

            row = 0;
            int lettersCount = letters.Length;
            for (int letterIndex = lettersCount; letterIndex > 0; letterIndex--)
            {
                int multiplierBase = (int)Math.Pow(alphaBase, letterIndex - 1);
                char c = letters[(lettersCount - letterIndex)];// INDEX IN REVERSE ORDER
                int baseAlpha = ConvertFromBaseAlphaChar(c, letterIndex);
                row += (baseAlpha * multiplierBase);
            }

            column = number - 1;
        }

        public static char ConvertToBaseAlphaChar(int baseAlphaNumber, int letterIndex)
        {
            if (baseAlphaNumber < 0)
            {
                throw new ArgumentException("baseAlphaNumber " + baseAlphaNumber);
            }
            char c = (char)('A' + baseAlphaNumber);

            if (letterIndex != 1)// this is because we don't have a 0 in the odd A, AA, AAA convention!!!!!!
            {
                c = (char)(c - 1);
            }

            if (c > Constants.ZUpperChar)
            {
                throw new ArgumentException("baseAlphaNumber " + baseAlphaNumber);
            }

            return c;
        }

        public static int ConvertFromBaseAlphaChar(char c, int letterIndex)
        {
            int val = (c - 'A');

            if (letterIndex != 1)// this is because we don't have a 0 in the odd A, AA, AAA convention!!!!!!
            {
                val++;
            }

            return val;
        }

        public static SBSWellAddress ToAddress(int row, int column)
        {
            string letter;
            int number;
            GetLetterAndNumber(row, column, out letter, out number);
            return new SBSWellAddress(letter.ToString(), number);
        }

        /// <summary>
        /// Creates "A1" etc from 0 based row/column
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string ToAddressString(int row, int column)
        {
            string letter;
            int number;
            GetLetterAndNumber(row, column, out letter, out number);
            return ToAddressString(letter.ToString(), number);
        }

        public static string ToAddressString(string letters, int number)
        {
            return letters + number;
        }

        ///<summary>
        ///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override string ToString()
        {
            return ToAddressString(m_letters, m_number);
        }

        /// <summary>
        /// creates a CSV list like A1,A2,A3 ... to compliment method <see cref="ParseList"/>
        /// </summary>
        /// <param name="wells"></param>
        /// <returns></returns>
        public static string ToStringList(IList<SBSWellAddress> wells)
        {
            return StringUtils.Join_(wells, Constants.CommaString);
        }

        public static void GetRowAndColumnCountForWellCount(int wellCount, out int rowCount, out int columnCount)
        {
            // row/col format is like this
            // wellCount = b * ( b * 2/3 )
            // b = SQRT( ( 3*wellCount )/2 )

            int b = (int)Math.Floor(Math.Sqrt((wellCount * 3) / 2));
            columnCount = b;
            rowCount = (b * 2) / 3;
        }

        public static int GetRowCount(int wellCount)
        {
            int rowCount;
            int columnCount;
            GetRowAndColumnCountForWellCount(wellCount, out rowCount, out columnCount);
            return rowCount;
        }

        public static int GetColumnCount(int wellCount)
        {
            int rowCount;
            int columnCount;
            GetRowAndColumnCountForWellCount(wellCount, out rowCount, out columnCount);
            return columnCount;
        }

        /// <summary>
        /// By convention ( UK Robotics ) maps A,B,C into X axis
        /// </summary>
        public static int GetXAxisCount(int wellCount)
        {
            return GetRowCount(wellCount);
        }

        /// <summary>
        /// By convention ( UK Robotics ) maps 1,2,3 ( ie number after the letter ) into Y axis
        /// </summary>
        public static int GetYAxisCount(int wellCount)
        {
            return GetColumnCount(wellCount);
        }

        public static SBSWellAddress GetLastWellAddress(int wellCount)
        {
            int rowCount = GetRowCount(wellCount);
            int columnCount = GetColumnCount(wellCount);
            return SBSWellAddress.ToAddress(rowCount - 1, columnCount - 1);
        }

        public static List<SBSWellAddress> GetRowAddressIterator(int wellCount, int columnIndex)
        {
            List<SBSWellAddress> addresses = new List<SBSWellAddress>();
            int rowCount = GetRowCount(wellCount);
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                addresses.Add(MatrixToSBSWellAddress(rowIndex, columnIndex));
            }

            return addresses;
        }

        public static List<SBSWellAddress> GetColumnAddressIterator(int wellCount, int rowIndex)
        {
            List<SBSWellAddress> addresses = new List<SBSWellAddress>();
            int columnCount = GetColumnCount(wellCount);
            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                addresses.Add(MatrixToSBSWellAddress(rowIndex, columnIndex));
            }

            return addresses;
        }

        public static List<SBSWellAddress> GetAddressIterator(int wellCount)
        {
            List<SBSWellAddress> addresses = new List<SBSWellAddress>();
            //int rowCount = GetRowCount(wellCount);
            int columnCount = GetColumnCount(wellCount);
            for (int column = 0; column < columnCount; column++)
            {
                addresses.AddRange(GetRowAddressIterator(wellCount, column));
            }
            return addresses;
        }

        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            SBSWellAddress otherAddress = obj as SBSWellAddress;
            if (null == otherAddress)
            {
                return false;
            }

            if (!m_letters.Equals(otherAddress.m_letters))
            {
                return false;
            }

            if (!m_number.Equals(otherAddress.m_number))
            {
                return false;
            }

            return true;
        }


        ///<summary>
        ///Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool AddressWithinMatrix(int wellCount, SBSWellAddress sbsWellAddress)
        {
            int rowCount, columnCount;
            GetRowAndColumnCountForWellCount(wellCount, out rowCount, out columnCount);

            if (sbsWellAddress.Row < rowCount && sbsWellAddress.Column < columnCount)
            {
                return true;
            }

            return false;
        }

    }
}