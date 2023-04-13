namespace Sat.Recruitment.Api.Domain
{
    internal class GifCalcuatorFactory
    {
        internal GifCalcuatorFactory()
        {
        }

        internal GifCalculator GetCalculator(UserType userType)
        {
            return userType switch
            {
                UserType.Normal => new NormalUserGifCalculator(),
                UserType.SuperUser => new SuperUserGifCalculator(),
                UserType.Premium => new PremiumUserGifCalculator(),
                _ => new NormalUserGifCalculator(),
            };
        }
    }
}