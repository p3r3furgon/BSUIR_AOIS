using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace AOIS_1{
    internal class Program{
        static void Main(string[] args) {
            Console.WriteLine("Enter first integer value: ");
            int value1 = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter second integer value: ");
            int value2 = int.Parse(Console.ReadLine());

            int[,] signs = { { 1, 1 }, { -1, -1 }, { -1, 1 }, { 1, -1 } };
            string sum, multiplication, division;
            for (int i = 0; i < 4; i++){
                var intValue1 = value1 * signs[i, 0];
                var intValue2 = value2 * signs[i, 1];
               if (intValue1 * intValue2 >= 0){
                    sum = Sum(PresentInDirectCode(intValue1, 16), PresentInDirectCode(intValue2, 16));
                    Console.WriteLine("value1 = " + intValue1 + " = " + PresentInDirectCode(intValue1, 16) + ", value2 = " + intValue2 + " = " + PresentInDirectCode(intValue2, 16) + ", sum = " + sum + " = " + ConvertFromBinaryToInt(sum));
                }
                else{
                    sum = DifferenceWithVariousSigns(PresentInDirectCode(intValue1, 16), PresentInDirectCode(intValue2, 16));
                    Console.WriteLine("value1 = " + intValue1 + " = " + PresentInDirectCode(intValue1, 16) + ", value2 = " + intValue2 + " = " + PresentInDirectCode(intValue2, 16) + ", sum = " + sum + " = " + ConvertFromBinaryToInt(sum));
                }
            }
            multiplication = Multiplication(PresentInDirectCode(value1, 16), PresentInDirectCode(value2, 16));
            division = Division(PresentInDirectCode(value1, 16), PresentInDirectCode(value2, 16));
            Console.WriteLine("Multiplication = " + multiplication + " = " + ConvertFromBinaryToInt(multiplication));
            Console.WriteLine("Division = " + division + " = " + ConvertFromBinaryToInt(division));

            while (true){
                Console.WriteLine("Enter first decimal value: ");
                decimal decimalValue1 = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Enter second decimal value: ");
                decimal decimalValue2 = decimal.Parse(Console.ReadLine());
                string binaryDecimaValue1 = ConvertFromDecimalToBinary(decimalValue1);
                string binaryDecimaValue2 = ConvertFromDecimalToBinary(decimalValue2);
                string sumOfDecimalValues = SumOfValuesWithFloatingPoint(binaryDecimaValue1, binaryDecimaValue2);
                Console.WriteLine(decimalValue1 + " = " + binaryDecimaValue1 + " = " + ConvertFromBinaryToDecimal(binaryDecimaValue1) + "\n" + decimalValue2 + " = " + binaryDecimaValue2 + " = " + ConvertFromBinaryToDecimal(binaryDecimaValue2) + "\nsum_function = " + sumOfDecimalValues + " = " + ConvertFromBinaryToDecimal(sumOfDecimalValues));
                Console.WriteLine("converter = " + ConvertFromDecimalToBinary(decimalValue1 + decimalValue2) + " = " + ConvertFromBinaryToDecimal(ConvertFromDecimalToBinary(decimalValue1 + decimalValue2)));
            }
            //int value = int.Parse(Console.ReadLine());
            //string binaryValue = PresentInDirectCode(value, 8);
            //Console.WriteLine(value + " " + binaryValue + " " + ConvertFromBinaryToInt(binaryValue));
        }

        static string GetAbs(string binaryValue){
            StringBuilder absValue = new StringBuilder(binaryValue);
            if (binaryValue[0] == '1'){
                absValue[0] = '0';
            }
            return absValue.ToString();
        }

        static string PresentInDirectCode(int value, int numberOfBits){

            var zerosPart = (numberOfBits - Convert.ToString(Math.Abs(value), 2).Length);
            var binaryValue = "";
            for (int i = 0; i < zerosPart; i++){
                if (i == 0 && value >= 0){
                    binaryValue += "0";
                }
                else if (i == 0 && value < 0){
                    binaryValue += "1";
                }
                else{
                    binaryValue += "0";
                }
            }
            return binaryValue + Convert.ToString(Math.Abs(value), 2);
        }

        static int ConvertFromBinaryToInt(string binaryValue) {
            int result = 0;
            for(int i = 0; i < binaryValue.Length-1; i++) {
                result += (int)(Math.Pow(2, i) * char.GetNumericValue(binaryValue[binaryValue.Length-1-i]));
            }
            if(binaryValue[0] == '1') {
                result *= -1;
            }
            return result;
        }
        static string PresentInReverseCode(int value, int numberOfBits){
            var directCode = PresentInDirectCode(value, numberOfBits);
            var reverseBinary = new StringBuilder();
            for (int i = 0; i < directCode.Length; i++){
                if (directCode[i] == '0'){
                    reverseBinary.Append('1');
                }
                else{
                    reverseBinary.Append('0');
                }
            }
            return reverseBinary.ToString();
        }

        static string PresentInReverseCode(string directCode){
            var reverseBinary = new StringBuilder();
            for (int i = 0; i < directCode.Length; i++){
                if (directCode[i] == '0'){
                    reverseBinary.Append('1');
                }
                else{
                    reverseBinary.Append('0');
                }
            }
            return reverseBinary.ToString();
        }

        static string PresentInAddictiveCode(int value, int numberOfBits){
            return Sum(PresentInReverseCode(value, numberOfBits), PresentInDirectCode(1, numberOfBits));
        }

        static string PresentInAddictiveCode(string directCode, int numberOfBits){
            return Sum(PresentInReverseCode(directCode), PresentInDirectCode(1, numberOfBits));
        }
        static string Sum(string binaryValue1, string binaryValue2){
            var reverseResult = new StringBuilder();
            var addValue = 0;
            for(var i = binaryValue1.Length - 1; i >= 0; i--){
                if (i != 0 || binaryValue1[0] != binaryValue2[0]){
                    if (char.GetNumericValue(binaryValue1[i]) + char.GetNumericValue(binaryValue2[i]) + addValue <= 1){
                        reverseResult.Append(((char.GetNumericValue(binaryValue1[i]) + char.GetNumericValue(binaryValue2[i]) + addValue).ToString()));
                        addValue = 0;
                    }
                    else{
                        reverseResult.Append(((char.GetNumericValue(binaryValue1[i]) + char.GetNumericValue(binaryValue2[i]) + addValue) % 2).ToString());
                        addValue = 1;
                    }
                }
                else {
                    if (IsAbsGreaterOrEqualThan(binaryValue1, binaryValue2)){
                        reverseResult.Append(binaryValue1[0]);
                    }
                    else{
                        reverseResult.Append(binaryValue2[0]);
                    }
                }
            }
            char[] result = reverseResult.ToString().ToCharArray();
            Array.Reverse(result);
            return new string(result);
        }

        static string DifferenceWithVariousSigns(string binaryValue1, string binaryValue2)
        {
            string minuend, subtrahend;
            if (binaryValue1[0] == '0'){
                minuend = binaryValue1;
                subtrahend = binaryValue2;
            }else 
            {
                minuend = binaryValue2;
                subtrahend = binaryValue1;
            }
            if (IsAbsGreaterOrEqualThan(minuend, subtrahend)){
                return Sum(minuend, PresentInAddictiveCode(subtrahend, binaryValue1.Length));  
            }
            else{
                return PresentInReverseCode(Sum(PresentInReverseCode(subtrahend), minuend)); 
            }
        }

        static string DifferenceWithSameSigns(string minuend, string subtrahend, int numberOfBits)
        {
            subtrahend =  subtrahend.Remove(0,1).Insert(0,"1");
            if (IsAbsGreaterOrEqualThan(minuend, subtrahend)){
                return Sum(minuend, PresentInAddictiveCode(subtrahend, numberOfBits));
            }
            else{
                return PresentInReverseCode(Sum(PresentInReverseCode(subtrahend), minuend));
            }
        }

        static bool IsAbsGreaterOrEqualThan(string binaryValue1, string binaryValue2){
           for (var i = 1; i < binaryValue1.Length; i++){
                if (binaryValue1[i] == '1' && binaryValue2[i] == '0'){
                    return true;
                }
                else if (binaryValue1[i] == '0' && binaryValue2[i] == '1'){
                    return false;
                }
           }
            return true;
        }

        static string LeftShiftBinaryValue(string binaryValue, int shiftValue, bool withNumber){
            string postZeros = "";
            StringBuilder result = new StringBuilder(binaryValue);
            for (var i = 0; i < shiftValue; i++){
                postZeros += '0';
            }
            if (!withNumber){
                result.Remove(1, shiftValue);
            }
            else{
                result.Remove(0, shiftValue);
            }
            result.Insert(result.Length, postZeros);
            return result.ToString();
        }

        static string Multiplication(string binaryValue1, string binaryValue2){
            var absResult = PresentInDirectCode(0, binaryValue1.Length);
            var absValue1 = GetAbs(binaryValue1);
            var absValue2 = GetAbs(binaryValue2);
            string copyValue1;
            StringBuilder postZeros = new StringBuilder();
            for (var i = absValue2.Length - 1; i >= 0; i--){
                copyValue1 = absValue1;
                if (i != absValue2.Length - 1){
                    postZeros.Append("0");
                }
                var tmpValue1 = copyValue1.Insert(binaryValue1.Length, postZeros.ToString());
                var tmpValue2 = tmpValue1.Remove(0, binaryValue1.Length - 1 - i);
                if (absValue2[i] == '1'){
                    absResult = Sum(absResult, tmpValue2);
                }
            }
            var result = new StringBuilder(absResult);
            if (binaryValue1[0] != binaryValue2[0]){
                result[0] = '1';
            }
            return result.ToString();
        }

        static string Division(string binaryValue1, string binaryValue2){
            string dividend, divider, remainder;
            StringBuilder result = new StringBuilder();
            int numberOfOperations = CalculatingNumberOfOperations(binaryValue1, binaryValue2);
            if (IsAbsGreaterOrEqualThan(binaryValue1, binaryValue2)){
                dividend = GetAbs(binaryValue1);
                divider = GetAbs(binaryValue2);
            }
            else{
                dividend = GetAbs(binaryValue2);
                divider = GetAbs(binaryValue1);
            }
            divider = LeftShiftBinaryValue(divider, numberOfOperations, false);
            remainder = DifferenceWithSameSigns(dividend, divider, binaryValue1.Length);
            if (remainder[0] == '0'){
                result.Append('1');
            }
            else { 
                result.Append('0');
            }
            for(var i = 0; i < numberOfOperations; i++){
                remainder = LeftShiftBinaryValue(remainder, 1, false);
                if (remainder[0] != divider[0]){
                    remainder = DifferenceWithVariousSigns(remainder, divider);
                }
                else {
                    remainder = DifferenceWithSameSigns(remainder, divider, binaryValue1.Length);
                }
                if (remainder[0] == '0')
                    result.Append('1');
                else
                    result.Append ('0');
            }
            return CreateRemainderForm(result.ToString(), binaryValue1, binaryValue2, binaryValue1.Length);
        }

        static int CalculatingNumberOfOperations(string binaryValue1, string binaryValue2){
            int index1 = 0, index2 = 0;
            for (var i = 1; i < binaryValue1.Length; i++){
                if (binaryValue1[i] == '1'){
                    break;
                }
                else{
                    index1++;
                }
            }
            for (var i = 1; i < binaryValue2.Length; i++){
                if (binaryValue2[i] == '1'){
                    break;
                }
                else{
                    index2++;
                }
            }
            return Math.Abs(index1 - index2);
        }

        static string CreateRemainderForm(string remainder, string dividend, string divider, int numberOfBits){
            var zerosPart = (numberOfBits - remainder.Length);
            var binaryValue = "";
            for (int i = 0; i < zerosPart; i++)
            {
                if (i == 0 && dividend[0] == divider[0]){
                    binaryValue += "0";
                }
                else if (i == 0 && dividend[0] != divider[0]){
                    binaryValue += "1";
                }
                else{
                    binaryValue += "0";
                }
            }
            return binaryValue + remainder;
        }

        static string ConvertFromDecimalToBinary(decimal value){
            string integerPart = "", fractalPart, strValue = Math.Abs(value).ToString(), sign, fractalPartZeros = "";
            int order = 0;
            if(value >= 0) {
                sign = "0";
            }
            else {
                sign = "1";
            }
            for (int i = 0; i < strValue.Length; i++) {
                if (strValue[i] == '.'){
                    break;
                }
                else{
                    integerPart += strValue[i];
                }
            }
            fractalPart = '0' + strValue.Substring(integerPart.Length);
            string fractalBinaryForm = MovingPoint(Convert.ToString(int.Parse(integerPart), 2) + "." + ConvertFractalPart(fractalPart), ref order);
            for(int i = 0; i < 23 - fractalBinaryForm.Length; i++) {
                fractalPartZeros += '0';
            }
            fractalBinaryForm += fractalPartZeros;
            fractalBinaryForm = fractalBinaryForm.Remove(23, fractalBinaryForm.Length - 23);
            return sign + ExponentOrderCalaculation(order) + fractalBinaryForm;
        }

        static string ConvertFractalPart(string fractalPart) {
            decimal value = decimal.Parse(fractalPart) * 2;
            string binaryFractalPart = "";
            for (int i = 0; i < 23; i += 1) {
                if (value == Math.Truncate(value)){
                    binaryFractalPart += value.ToString()[0];
                    break;
                }
                else {
                    binaryFractalPart += value.ToString()[0];
                    value = value * 2 % 2;
                }   
            }
            return binaryFractalPart;
        }

        static string MovingPoint(string rawBinaryForm, ref int order) {
            int oldPointIndex = rawBinaryForm.IndexOf(".");
            int newPointIndex = 0;
            if (rawBinaryForm[0] == '1'){
                for (int i = 0; i < rawBinaryForm.Length; i++){
                    if (rawBinaryForm[i] == '1'){
                        rawBinaryForm = rawBinaryForm.Replace(".", "");
                        rawBinaryForm = rawBinaryForm.Insert(i + 1, ".");
                        newPointIndex = i + 1;
                        break;
                    }
                }
            }
            else {
                for (int i = 0; i < rawBinaryForm.Length; i++)
                {
                    if (rawBinaryForm[i] == '1')
                    {
                        rawBinaryForm = rawBinaryForm.Replace(".", "");
                        rawBinaryForm = rawBinaryForm.Insert(i, ".");
                        newPointIndex = i;
                        break;
                    }
                }
            }
            rawBinaryForm = rawBinaryForm.Remove(0, newPointIndex+1);
            //rawBinaryForm = rawBinaryForm.Remove(23, rawBinaryForm.Length - 23);
            order = oldPointIndex - newPointIndex;
            return rawBinaryForm;
        }

        static string ExponentOrderCalaculation(int order) {
            return PresentInDirectCode(order + 127, 8);
        }

        static string MantissasRightShift(string binaryValue, int shiftValue, bool withUnseenBit){
            string postZeros = "";
            int unseenBitShift = 0;
            if(withUnseenBit){
                unseenBitShift = 1;
            }
            StringBuilder result = new StringBuilder(binaryValue);
            for (var i = 0; i < shiftValue; i++)
            {
                postZeros += '0';
            }
            result.Remove(binaryValue.Length - 1 - shiftValue, shiftValue + unseenBitShift);
            result.Insert(0, postZeros);
            return result.ToString();
        }

        static string SumOfValuesWithFloatingPoint(string binaryValue1, string binaryValue2) {
            string[] signs = { binaryValue1[0].ToString(), binaryValue2[0].ToString() };
            string[] exponents = { binaryValue1.Substring(1, 8), binaryValue2.Substring(1, 8) };
            string[] mantissas = { binaryValue1.Substring(9,23), binaryValue2.Substring(9,23)};
            string[] unseenBits = { "1", "1" };
            int shiftValue = Math.Abs(ConvertFromBinaryToInt(DifferenceWithSameSigns(signs[0] + exponents[0], signs[1] + exponents[1],9)));
            if (IsAbsGreaterOrEqualThan(signs[0] + exponents[0], signs[1] + exponents[1]) && shiftValue != 0){
                exponents[1] = exponents[0];
                mantissas[1] = MantissasRightShift(unseenBits[1] + mantissas[1], shiftValue-1, true);
                unseenBits[1] = "0";
            }
            else if(!IsAbsGreaterOrEqualThan(signs[0] + exponents[0], signs[1] + exponents[1])){
                exponents[0] = exponents[1];
                mantissas[0] = MantissasRightShift(unseenBits[0] + mantissas[0], shiftValue-1, true);
                unseenBits[0] = "0";
            }
            string resMantissa = SumOfMantissas(mantissas, unseenBits, ref exponents[0], signs[0]);
            return signs[0] + exponents[0] + resMantissa;
        }

        static string SumOfMantissas(string[] mantissas, string[] unseenBits, ref string exponent, string sign) {
            var reverseResult = new StringBuilder();
            var addValue = 0;
            bool isShifted;
            for (var i = mantissas[0].Length - 1; i >= 0; i--){
                if (char.GetNumericValue(mantissas[0][i]) + char.GetNumericValue(mantissas[1][i]) + addValue <= 1){
                    reverseResult.Append(((char.GetNumericValue(mantissas[0][i]) + char.GetNumericValue(mantissas[1][i]) + addValue).ToString()));
                    addValue = 0;
                }
                else{
                    reverseResult.Append(((char.GetNumericValue(mantissas[0][i]) + char.GetNumericValue(mantissas[1][i]) + addValue) % 2).ToString());
                    addValue = 1;
                }
            }
            char[] result = reverseResult.ToString().ToCharArray();
            Array.Reverse(result);
            if (char.GetNumericValue(unseenBits[0][0]) + char.GetNumericValue(unseenBits[1][0]) + addValue >= 2){
                exponent = Sum(sign + exponent, PresentInDirectCode(1, 9)).Remove(0,1);
                string mantissasSum = new string(result).Insert(0, ((char.GetNumericValue(unseenBits[0][0]) + char.GetNumericValue(unseenBits[1][0]) + addValue)%2).ToString());
                mantissasSum = mantissasSum.Remove(mantissasSum.Length - 1, 1);
                return mantissasSum;
            }
            else {
                return new string(result);
            }
        }

        static decimal ConvertFromBinaryToDecimal(string binaryValue) {
            string sign = binaryValue[0].ToString();
            string exponent = binaryValue.Substring(1, 8);
            string mantissa = "1" + binaryValue.Substring(9, 23);
            int decimalDegree = ConvertFromBinaryToInt(sign + exponent) - 127;
            double digitDegree = 0;
            decimal mantissaResult = 0;
            for(int i = 0; i < mantissa.Length; i +=1){
                mantissaResult += (decimal)(char.GetNumericValue(mantissa[i]) * Math.Pow(2, digitDegree));
                digitDegree -= 1;
            }
            return (decimal)Math.Pow(-1, char.GetNumericValue(sign[0])) * mantissaResult * (decimal)Math.Pow(2, decimalDegree);
        }
    }
}