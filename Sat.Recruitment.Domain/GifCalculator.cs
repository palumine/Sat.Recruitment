namespace Sat.Recruitment.Domain
{
    internal abstract class GifCalculator
    {
        /// <summary>
        /// Returns the money plus gif applied to the money
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        internal abstract decimal Calculate(decimal money);
    }

    internal class NormalUserGifCalculator : GifCalculator
    {
        internal override decimal Calculate(decimal money)
        {
            var percentage = 0m;
            if (money > 100)
            {
                //If new user is normal and has more than USD100
                percentage = 0.12m;
            }
            if (money <= 100) //TODO verify equals to 100 condition
            {
                if (money > 10)
                {
                    percentage = 0.8m;
                }
            }
            return money * (1 + percentage);
        }
    }

    internal class SuperUserGifCalculator : GifCalculator
    {
        internal override decimal Calculate(decimal money)
        {
            var percentage = 0m;
            if (money > 100)
            {
                percentage = 0.20m;
            }
            return money * (1 + percentage);
        }
    }

    internal class PremiumUserGifCalculator : GifCalculator
    {
        internal override decimal Calculate(decimal money)
        {
            if (money > 100)
            {
                return money * 2;
            }
            return money;
        }
    }
}