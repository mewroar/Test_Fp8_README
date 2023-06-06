using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;


namespace Gitan.FixedPoint8;

public readonly struct FixedPoint8 : INumber<FixedPoint8>
{

    public static FixedPoint8 MaxValue { get; } = new FixedPoint8(long.MaxValue);
    public static FixedPoint8 MinValue { get; } = new FixedPoint8(long.MinValue);

    public const int InnerPower = 100_000_000;

    readonly long _innerValue = 0;
    public long InnerValue => _innerValue;

    public static FixedPoint8 Zero { get; } = new FixedPoint8(0);

    public static FixedPoint8 One { get; } = new FixedPoint8(InnerPower);

    public static int Radix => 10;

    public static FixedPoint8 AdditiveIdentity => Zero;

    public static FixedPoint8 MultiplicativeIdentity => One;

    public FixedPoint8(long innerValue)
    {
        _innerValue = innerValue;
    }

    // ****************************************
    // FixedPoint8への変換
    // ****************************************

    public static FixedPoint8 FromInnerValue(long value)
    {
        return new FixedPoint8(value);
    }

    public static FixedPoint8 FromDouble(double value)
    {
        return new FixedPoint8((long)(value * InnerPower));
    }

    public static FixedPoint8 FromDecimal(decimal value)
    {
        return new FixedPoint8((long)(value * InnerPower));
    }

    // 速度最適化は未実施
    public static FixedPoint8 Parse(string s)
    {
        var parse = decimal.Parse(s);
        return FromDecimal(parse);
    }

    // 速度最適化は未実施
    public static FixedPoint8 Parse(ReadOnlySpan<char> s)
    {
        var result = decimal.Parse(s);
        return FromDecimal(result);
    }

    public static FixedPoint8 Parse(ReadOnlySpan<byte> utf8)
    {

        if (TryParse(utf8, out var result))
        {
            return result;
        }
        throw new FormatException("正しい形式ではありません");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, out FixedPoint8 result)
    {
        if (decimal.TryParse(s, out var num))
        {
            result = FromDecimal(num);
            return true;
        }

        result = FixedPoint8.Zero;
        return false;
    }

    // 速度最適化は未実施
    public static bool TryParse(ReadOnlySpan<char> s, out FixedPoint8 result)
    {

        if (decimal.TryParse(s, out var num))
        {
            result = FromDecimal(num);
            return true;
        }

        result = FixedPoint8.Zero;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<byte> utf8, out FixedPoint8 result)
    {
        int offset = 0;
        int sign = 1; // 1 = + ,-1 = -
        long overPoint = 0;
        long underPoint = 0;

        if (utf8.Length == 0) { goto returnFalse; }

        // + - の処理
        if (utf8[0] == (byte)'-')
        {
            offset++;
            sign = -1;
        }
        else if (utf8[0] == (byte)'+')
        {
            offset++;
        }

        // 0～9の処理
        while (offset < utf8.Length && utf8[offset] >= (byte)'0' && utf8[offset] <= (byte)'9')
        {
            int digit = (int)(utf8[offset] - (byte)'0');
            offset++;
            overPoint = overPoint * 10 + digit;
        }

        // .の処理
        if (offset >= utf8.Length)
        {
            goto returnTrue;
        }
        if (utf8[offset] == (byte)'.')
        {
            offset++;

            long power = 100_000_000_000_000_000;
            while (offset < utf8.Length && utf8[offset] >= (byte)'0' && utf8[offset] <= (byte)'9')
            {
                long digit = (long)(utf8[offset] - (byte)'0');
                offset++;

                underPoint += digit * power;
                power /= 10;
            }
        }

        // 最後だったら
        if (offset >= utf8.Length)
        {
            goto returnTrue;
        }

        // e,Eじゃなかったら
        if (utf8[offset] != (byte)'e' && utf8[offset] != (byte)'E')
        {
            goto returnFalse;
        }

        // e,Eだったら
        int powerSignPower = 1;
        int powerNum = 0;
        offset++;

        if (utf8[offset] == (byte)'-')
        {
            offset++;
            powerSignPower = -1;
        }
        else if (utf8[offset] == (byte)'+')
        {
            offset++;
        }

        while (offset < utf8.Length)
        {
            if (utf8[offset] >= (byte)'0' && utf8[offset] <= (byte)'9')
            {
                int digit = (int)(utf8[offset] - (byte)'0');
                offset++;
                powerNum = powerNum * 10 + digit;
            }
            else
            {
                goto returnFalse;
            }
        }

        powerNum *= powerSignPower;

        var powerNumOver = powerNum + 8;
        long calcOver;
        if (powerNumOver >= 0)
        {
            calcOver = overPoint * powerArray[powerNumOver];
        }
        else
        {
            calcOver = overPoint / powerArray[-powerNumOver];
        }

        var powerNumUnder = powerNum - 10;
        long calcUnder;
        if (powerNumUnder >= 0)
        {
            calcUnder = underPoint * powerArray[powerNumUnder];
        }
        else
        {
            if (powerNumUnder < -18)
            {
                calcUnder = 0;
            }
            else
            {
                calcUnder = underPoint / powerArray[-powerNumUnder];
            }
        }

        result = new FixedPoint8((calcOver + calcUnder) * sign);
        return true;


    returnTrue:
        result = new FixedPoint8((overPoint * InnerPower + underPoint / 10_000_000_000) * sign);
        return true;

    returnFalse:
        result = FixedPoint8.Zero;
        return false;
    }

    // ****************************************
    // FixedPoint8からの変換
    // ****************************************

    // 速度最適化は未実施
    public override string ToString()
    {
        return ((decimal)_innerValue / InnerPower).ToString();
    }

    public byte[] ToUtf8()
    {
        Span<byte> buffer = stackalloc byte[21];

        int offset = 0;
        uint num1, num2, num3, div;
        ulong valueA, valueB;


        if (_innerValue < 0)
        {
            if (_innerValue == long.MinValue)
            {
                ReadOnlySpan<byte> minValue = "-92233720368.54775808"u8;
                return minValue.ToArray();
            }

            buffer[offset++] = (byte)'-';
            valueB = (ulong)(unchecked(-_innerValue));
        }
        else
        {
            valueB = (ulong)_innerValue;
        }

        valueA = valueB / InnerPower;

        var underPoint = valueB - (valueA * InnerPower);

        if (valueA < 10000)
        {
            num1 = (uint)valueA;
            if (num1 < 10) { goto L1; }
            if (num1 < 100) { goto L2; }
            if (num1 < 1000) { goto L3; }
            goto L4;
        }
        else
        {
            valueB = valueA / 10000;
            num1 = (uint)(valueA - valueB * 10000);
            if (valueB < 10000)
            {
                num2 = (uint)valueB;
                if (num2 < 100)
                {
                    if (num2 < 10) { goto L5; }
                    goto L6;
                }
                if (num2 < 1000) { goto L7; }
                goto L8;
            }
            else
            {
                valueA = valueB / 10000;
                num2 = (uint)(valueB - valueA * 10000);
                {
                    num3 = (uint)valueA;
                    if (num3 < 100)
                    {
                        if (num3 < 10) { goto L9; }
                        goto L10;
                    }
                    if (num3 < 1000) { goto L11; }
                    goto L12;

                L12:
                    buffer[offset++] = (byte)('0' + (div = (num3 * 8389) >> 23));
                    num3 -= div * 1000;
                L11:
                    buffer[offset++] = (byte)('0' + (div = (num3 * 5243) >> 19));
                    num3 -= div * 100;
                L10:
                    buffer[offset++] = (byte)('0' + (div = (num3 * 6554) >> 16));
                    num3 -= div * 10;
                L9:
                    buffer[offset++] = (byte)('0' + (num3));
                }
            }
        L8:
            buffer[offset++] = (byte)('0' + (div = (num2 * 8389) >> 23));
            num2 -= div * 1000;
        L7:
            buffer[offset++] = (byte)('0' + (div = (num2 * 5243) >> 19));
            num2 -= div * 100;
        L6:
            buffer[offset++] = (byte)('0' + (div = (num2 * 6554) >> 16));
            num2 -= div * 10;
        L5:
            buffer[offset++] = (byte)('0' + (num2));

        }

    L4:
        buffer[offset++] = (byte)('0' + (div = (num1 * 8389) >> 23));
        num1 -= div * 1000;
    L3:
        buffer[offset++] = (byte)('0' + (div = (num1 * 5243) >> 19));
        num1 -= div * 100;
    L2:
        buffer[offset++] = (byte)('0' + (div = (num1 * 6554) >> 16));
        num1 -= div * 10;
    L1:
        buffer[offset++] = (byte)('0' + (num1));


        if (underPoint > 0)
        {

            buffer[offset++] = (byte)'.';

            while (underPoint > 0)
            {
                byte num = (byte)(underPoint / 10_000_000);
                buffer[offset++] = (byte)('0' + num);

                underPoint = underPoint * 10 - (ulong)num * InnerPower;
            }
        }

        return buffer[..offset].ToArray();

    }

    // ****************************************
    // FixedPoint8への変換(cast)
    // ****************************************

    public static explicit operator FixedPoint8(sbyte value)
    {
        return new FixedPoint8((long)value * (long)InnerPower);
    }

    public static explicit operator FixedPoint8(byte value)
    {
        return new FixedPoint8((long)value * (long)InnerPower);
    }

    public static explicit operator FixedPoint8(short value)
    {
        return new FixedPoint8((long)value * (long)InnerPower);
    }

    public static explicit operator FixedPoint8(ushort value)
    {
        return new FixedPoint8((long)value * (long)InnerPower);
    }

    public static explicit operator FixedPoint8(int value)
    {
        return new FixedPoint8((long)value * (long)InnerPower);
    }

    public static explicit operator FixedPoint8(uint value)
    {
        return new FixedPoint8((long)value * (long)InnerPower);
    }

    public static explicit operator FixedPoint8(long value)
    {
        return new FixedPoint8(value * (long)InnerPower);
    }

    public static explicit operator FixedPoint8(ulong value)
    {
        return new FixedPoint8((long)value * (long)InnerPower);
    }

    public static explicit operator FixedPoint8(float value)
    {
        return new FixedPoint8((long)(value * InnerPower));
    }

    public static explicit operator FixedPoint8(double value)
    {
        return FromDouble(value);
    }

    public static explicit operator FixedPoint8(decimal value)
    {
        return FromDecimal(value);
    }

    // ****************************************
    // FixedPoint8からの変換(cast)
    // ****************************************

    public static explicit operator sbyte(FixedPoint8 value)
    {
        return (sbyte)(value._innerValue / InnerPower);
    }

    public static explicit operator byte(FixedPoint8 value)
    {
        return (byte)(value._innerValue / InnerPower);
    }

    public static explicit operator short(FixedPoint8 value)
    {
        return (short)(value._innerValue / InnerPower);
    }

    public static explicit operator ushort(FixedPoint8 value)
    {
        return (ushort)(value._innerValue / InnerPower);
    }

    public static explicit operator int(FixedPoint8 value)
    {
        return ((int)value._innerValue / InnerPower);
    }

    public static explicit operator uint(FixedPoint8 value)
    {
        return (uint)(value._innerValue / InnerPower);
    }

    public static explicit operator long(FixedPoint8 value)
    {
        return value._innerValue / InnerPower;
    }

    public static explicit operator ulong(FixedPoint8 value)
    {
        return (ulong)(value._innerValue / InnerPower);
    }

    public static explicit operator float(FixedPoint8 value)
    {
        return ((float)value._innerValue / InnerPower);
    }

    public static explicit operator double(FixedPoint8 value)
    {
        return ((double)value._innerValue / InnerPower);
    }

    public static explicit operator decimal(FixedPoint8 value)
    {
        return ((decimal)value._innerValue / InnerPower);
    }

    // ****************************************
    // operator
    // ****************************************

    public static FixedPoint8 operator +(FixedPoint8 left, FixedPoint8 right)
    {
        return new FixedPoint8(left._innerValue + right._innerValue);
    }

    public static FixedPoint8 operator -(FixedPoint8 left, FixedPoint8 right)
    {
        return new FixedPoint8(left._innerValue - right._innerValue);
    }


    public static FixedPoint8 operator *(FixedPoint8 left, long right)
    {
        return new FixedPoint8(left._innerValue * right);
    }

    public static FixedPoint8 operator *(FixedPoint8 left, ulong right)
    {
        return new FixedPoint8(left._innerValue * (long)right);
    }

    //速度が出ないので使用は推奨しない
    public static FixedPoint8 operator *(FixedPoint8 left, FixedPoint8 right)
    {
        var decimalLeft = (decimal)(left._innerValue) / InnerPower;
        var decimalRight = (decimal)(right._innerValue) / InnerPower;
        return FixedPoint8.FromDecimal(decimalLeft * decimalRight);
    }

    public static FixedPoint8 operator /(FixedPoint8 left, long right)
    {
        return new FixedPoint8(left._innerValue / right);
    }

    public static FixedPoint8 operator /(FixedPoint8 left, ulong right)
    {
        return new FixedPoint8(left._innerValue / (long)right);
    }

    //速度が出ないので使用は推奨しない
    public static FixedPoint8 operator /(FixedPoint8 left, FixedPoint8 right)
    {
        var decimalLeft = (decimal)(left._innerValue) / InnerPower;
        var decimalRight = (decimal)(right._innerValue) / InnerPower;
        return FixedPoint8.FromDecimal(decimalLeft / decimalRight);
    }

    public static bool operator ==(FixedPoint8 left, FixedPoint8 right)
    {
        return left._innerValue == right._innerValue;
    }

    public static bool operator !=(FixedPoint8 left, FixedPoint8 right)
    {
        return left._innerValue != right._innerValue;
    }

    public static bool operator <(FixedPoint8 left, FixedPoint8 right)
    {
        return left._innerValue < right._innerValue;
    }
    public static bool operator <=(FixedPoint8 left, FixedPoint8 right)
    {
        return left._innerValue <= right._innerValue;
    }

    public static bool operator >(FixedPoint8 left, FixedPoint8 right)
    {
        return left._innerValue > right._innerValue;
    }
    public static bool operator >=(FixedPoint8 left, FixedPoint8 right)
    {
        return left._innerValue >= right._innerValue;
    }

    public static FixedPoint8 operator %(FixedPoint8 left, FixedPoint8 right)
    {
        return new FixedPoint8(left._innerValue % right._innerValue);
    }

    public static FixedPoint8 operator ++(FixedPoint8 value)
    {
        return value + One;
    }

    public static FixedPoint8 operator --(FixedPoint8 value)
    {
        return value - One;
    }

    public static FixedPoint8 operator +(FixedPoint8 value)
    {
        return value;
    }

    public static FixedPoint8 operator -(FixedPoint8 value)
    {
        return new FixedPoint8(-value.InnerValue);
    }



    // ****************************************
    // その他
    // ****************************************

    public override bool Equals(object? obj)
    {
        return obj is FixedPoint8 fixedPoint8 &&
               _innerValue == fixedPoint8._innerValue;
    }

    public bool Equals(FixedPoint8 value)
    {
        return value._innerValue == _innerValue;
    }

    public override int GetHashCode()
    {
        return _innerValue.GetHashCode();
    }

    public int CompareTo(object? obj)
    {
        return obj is FixedPoint8 fixedPoint8 ? _innerValue.CompareTo(fixedPoint8._innerValue) : throw new ArgumentException("obj is not FixedPoint8.");
    }

    public int CompareTo(FixedPoint8 other)
    {
        return _innerValue.CompareTo(other._innerValue);
    }

    public static FixedPoint8 Abs(FixedPoint8 value)
    {
        if (value._innerValue < 0)
        {
            return new FixedPoint8(-value._innerValue);
        }
        return value;
    }

    public static bool IsCanonical(FixedPoint8 value)
    {
        return true;
    }

    public static bool IsComplexNumber(FixedPoint8 value)
    {
        return false;
    }

    public static bool IsEvenInteger(FixedPoint8 value)
    {
        long overPoint = value._innerValue / 200_000_000;
        long underPoint = value._innerValue - overPoint * 200_000_000;
        if (underPoint == 0)
        {
            return true;
        }
        return false;
    }

    public static bool IsFinite(FixedPoint8 value)
    {
        return true;
    }

    public static bool IsImaginaryNumber(FixedPoint8 value)
    {
        return false;
    }

    public static bool IsInfinity(FixedPoint8 value)
    {
        return false;
    }

    public static bool IsInteger(FixedPoint8 value)
    {
        long overPoint = value._innerValue / InnerPower;
        long underPoint = value._innerValue - overPoint * InnerPower;
        if (underPoint == 0)
        {
            return true;
        }
        return false;
    }

    public static bool IsNaN(FixedPoint8 value)
    {
        return false;
    }

    public static bool IsNegative(FixedPoint8 value)
    {
        return value._innerValue < 0;
    }

    public static bool IsNegativeInfinity(FixedPoint8 value)
    {
        return false;
    }

    public static bool IsNormal(FixedPoint8 value)
    {
        return true;
    }

    public static bool IsOddInteger(FixedPoint8 value)
    {
        long overPoint = value._innerValue / 200_000_000;
        long underPoint = value._innerValue - overPoint * 200_000_000;
        if (underPoint == InnerPower || underPoint == -InnerPower)
        {
            return true;
        }
        return false;
    }

    public static bool IsPositive(FixedPoint8 value)
    {
        return (value._innerValue >= 0);
    }

    public static bool IsPositiveInfinity(FixedPoint8 value)
    {
        return false;
    }

    public static bool IsRealNumber(FixedPoint8 value)
    {
        return true;
    }

    public static bool IsSubnormal(FixedPoint8 value)
    {
        return false;
    }

    public static bool IsZero(FixedPoint8 value)
    {
        return (value._innerValue == 0);
    }

    public static FixedPoint8 MaxMagnitude(FixedPoint8 x, FixedPoint8 y)
    {
        long absX = x._innerValue;

        if (absX < 0)
        {
            absX = -absX;

            if (absX < 0)
            {
                return x;
            }
        }

        long absY = y._innerValue;

        if (absY < 0)
        {
            absY = -absY;

            if (absY < 0)
            {
                return y;
            }
        }

        if (absX > absY)
        {
            return x;
        }

        if (absX == absY)
        {
            return IsNegative(x) ? y : x;
        }

        return y;
    }

    public static FixedPoint8 MaxMagnitudeNumber(FixedPoint8 x, FixedPoint8 y)
    {
        return MaxMagnitude(x, y);
    }

    public static FixedPoint8 MinMagnitude(FixedPoint8 x, FixedPoint8 y)
    {
        long absX = x._innerValue;

        if (absX < 0)
        {
            absX = -absX;

            if (absX < 0)
            {
                return y;
            }
        }

        long absY = y._innerValue;

        if (absY < 0)
        {
            absY = -absY;

            if (absY < 0)
            {
                return x;
            }
        }

        if (absX < absY)
        {
            return x;
        }

        if (absX == absY)
        {
            return IsNegative(x) ? x : y;
        }

        return y;
    }

    public static FixedPoint8 MinMagnitudeNumber(FixedPoint8 x, FixedPoint8 y)
    {
        return MinMagnitude(x, y);
    }

    public static FixedPoint8 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static FixedPoint8 Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out FixedPoint8 result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out FixedPoint8 result)
    {
        throw new NotImplementedException();
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        throw new NotImplementedException();
    }

    public static FixedPoint8 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out FixedPoint8 result)
    {
        throw new NotImplementedException();
    }

    public static FixedPoint8 Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out FixedPoint8 result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<FixedPoint8>.TryConvertFromChecked<TOther>(TOther value, out FixedPoint8 result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<FixedPoint8>.TryConvertFromSaturating<TOther>(TOther value, out FixedPoint8 result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<FixedPoint8>.TryConvertFromTruncating<TOther>(TOther value, out FixedPoint8 result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<FixedPoint8>.TryConvertToChecked<TOther>(FixedPoint8 value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<FixedPoint8>.TryConvertToSaturating<TOther>(FixedPoint8 value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<FixedPoint8>.TryConvertToTruncating<TOther>(FixedPoint8 value, out TOther result)
    {
        throw new NotImplementedException();
    }

    public FixedPoint8 Round()
    {
        long absValue;
        int sign; // 1 = + ,-1 = -

        if (_innerValue < 0)
        {
            absValue = -_innerValue;
            sign = -1;
        }
        else
        {
            absValue = _innerValue;
            sign = 1;
        }

        var absOverPoint = absValue / InnerPower;
        var absUnderPoint = absValue - absOverPoint * InnerPower;

        if (absUnderPoint == 50000000)
        {
            if ((absOverPoint & 1) == 0)
            {
                return new FixedPoint8(absOverPoint * InnerPower * sign);
            }
            else
            {
                return new FixedPoint8((1 + absOverPoint) * InnerPower * sign);
            }
        }
        var value = InnerValue + 50000000 * sign;
        var overPoint = value / InnerPower;

        return new FixedPoint8(overPoint * InnerPower);
    }

    //decimals = 2　→　小数点２桁になるように丸める
    //decimals = -2　→　100の単位になるように丸める

    //1234.56780000 , -2 → 1200.00000000
    //  12 , 345678000000000000

    //1234.56780000 , -3 → 1000.00000000
    //   1 , 234567800000000000

    //1500.00000000 , -3 → 2000.00000000
    //   1 , 500000000000000000

    public FixedPoint8 Round(int decimals)
    {
        if (decimals < -10 || decimals > 7)
        {
            throw new ArgumentOutOfRangeException();
        }

        long absValue;
        int sign; // 1 = + ,-1 = -

        if (_innerValue < 0)
        {
            absValue = -_innerValue;
            sign = -1;
        }
        else
        {
            absValue = _innerValue;
            sign = 1;
        }

        var overDiv = powerArray[-decimals + 8];
        var absOverRound = absValue / overDiv;

        var underPower = powerArray[10 + decimals];
        var absUnderRound = (absValue - absOverRound * overDiv) * underPower;

        if (absUnderRound == 500000000000000000)
        {
            if ((absOverRound & 1) == 0)
            {
                return new FixedPoint8(absOverRound * overDiv * sign) ;
            }
            return new FixedPoint8((absOverRound + 1) * overDiv * sign);
        }

        var mRoundOff = 5 * overDiv / 10;
        return new FixedPoint8(((absValue + mRoundOff) / overDiv) * overDiv * sign);
    }


    public FixedPoint8 Floor()
    {
        long absValue;
        bool sign; // true = + , false = -

        if (_innerValue < 0)
        {
            absValue = -_innerValue;
            sign = false;
        }
        else
        {
            absValue = _innerValue;
            sign = true;
        }

        var overPoint = absValue / InnerPower;
        var underPoint = absValue - overPoint * InnerPower;

        if (sign)
        {
            return new FixedPoint8(overPoint * InnerPower);
        }
        if (underPoint == 0)
        {
            return new FixedPoint8(-overPoint * InnerPower);
        }
        return new FixedPoint8(-(1 + overPoint) * InnerPower);

    }

    public FixedPoint8 Floor(int decimals)
    {
        if (decimals < -10 || decimals > 7)
        {
            throw new ArgumentOutOfRangeException();
        }

        long absValue;
        bool sign; // true = + , false = -

        if (_innerValue < 0)
        {
            absValue = -_innerValue;
            sign = false;
        }
        else
        {
            absValue = _innerValue;
            sign = true;

        }

        var overDiv = powerArray[-decimals + 8];
        var absOverRound = absValue / overDiv;

        var underPower = powerArray[10 + decimals];
        var absUnderRound = (absValue - absOverRound * overDiv) * underPower;

        if(sign)
        {
            return new FixedPoint8(absOverRound * overDiv);

        }
        if (absUnderRound == 0)
        {
            return new FixedPoint8(-absOverRound * overDiv);
        }
        return new FixedPoint8(-(absOverRound + 1) * overDiv);
    }


    public FixedPoint8 Truncate()
    {
        long absValue;
        int sign; // 1 = + ,-1 = -

        if (_innerValue < 0)
        {
            absValue = -_innerValue;
            sign = -1;
        }
        else
        {
            absValue = _innerValue;
            sign = 1;

        }

        var overPoint = absValue / InnerPower;

        return new FixedPoint8(overPoint * InnerPower * sign);
    }

    public FixedPoint8 Truncate(int decimals)
    {
        if (decimals < -10 || decimals > 7)
        {
            throw new ArgumentOutOfRangeException();
        }

        long absValue;
        int sign; // 1 = + ,-1 = -

        if (_innerValue < 0)
        {
            absValue = -_innerValue;
            sign = -1;
        }
        else
        {
            absValue = _innerValue;
            sign = 1;

        }

        var overDiv = powerArray[-decimals + 8];
        var absOverRound = absValue / overDiv;

        return new FixedPoint8(absOverRound * overDiv * sign);
    }


    public FixedPoint8 Ceiling()
    {
        long absValue;
        bool sign; // true = + , false = -

        if (_innerValue < 0)
        {
            absValue = -_innerValue;
            sign = false;
        }
        else
        {
            absValue = _innerValue;
            sign = true;
        }

        var overPoint = absValue / InnerPower;
        var underPoint = absValue - overPoint * InnerPower;

        if (sign)
        {
            if (underPoint == 0)
            {
                return new FixedPoint8(overPoint * InnerPower);
            }
            return new FixedPoint8((1 + overPoint) * InnerPower);
        }
        return new FixedPoint8(-overPoint * InnerPower);
    }

    public FixedPoint8 Ceiling(int decimals)
    {
        long absValue;
        bool sign; // true = + , false = -

        if (_innerValue < 0)
        {
            absValue = -_innerValue;
            sign = false;
        }
        else
        {
            absValue = _innerValue;
            sign = true;
        }

        var overDiv = powerArray[-decimals + 8];
        var absOverRound = absValue / overDiv;

        var underPower = powerArray[10 + decimals];
        var absUnderRound = (absValue - absOverRound * overDiv) * underPower;

        if (sign)
        {
            if (absUnderRound == 0)
            {
                return new FixedPoint8(absOverRound * overDiv);
            }
            return new FixedPoint8((absOverRound + 1) * overDiv);
        }
        return new FixedPoint8(-absOverRound * overDiv);
    }


    static readonly long[] powerArray = new long[] {
        1,
        10,
        100,
        1_000,
        10_000,
        100_000,
        1_000_000,
        10_000_000,
        100_000_000,
        1_000_000_000,
        10_000_000_000,
        100_000_000_000,
        1_000_000_000_000,
        10_000_000_000_000,
        100_000_000_000_000,
        1_000_000_000_000_000,
        10_000_000_000_000_000,
        100_000_000_000_000_000,
        1_000_000_000_000_000_000
    };
}








