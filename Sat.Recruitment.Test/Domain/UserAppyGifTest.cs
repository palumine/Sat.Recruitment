using Sat.Recruitment.Domain;
using System;
using Xunit;


namespace Sat.Recruitment.Test.Domain
{
    public class UserAppyGifTest
    {
        [Theory]
        [InlineData(UserType.Normal, "1000", "1120")]
        [InlineData(UserType.Normal, "100", "180")]
        [InlineData(UserType.Normal, "80", "144")]
        [InlineData(UserType.Normal, "5", "5")]
        [InlineData(UserType.SuperUser, "150", "180")]
        [InlineData(UserType.SuperUser, "100", "100")]
        [InlineData(UserType.SuperUser, "80", "80")]
        [InlineData(UserType.Premium, "150", "300")]
        [InlineData(UserType.Premium, "100", "100")]
        [InlineData(UserType.Premium, "50", "50")]
        public void ApplyGif_ShouldApplyGifByUserTypeCorrectly(UserType userType, string money, string moneyWithGifApplied)
        {
            var moneyPlusGif = Convert.ToDecimal(moneyWithGifApplied);
            var user = new User(string.Empty, string.Empty, string.Empty, string.Empty, userType, Convert.ToDecimal(money));

            Assert.Equal(moneyPlusGif, user.MoneyPlusGif);
        }

    }
}
