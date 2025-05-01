using System.Threading.Tasks;
using QLKho_NCKH.Models.TokenAuth;
using QLKho_NCKH.Web.Controllers;
using Shouldly;
using Xunit;

namespace QLKho_NCKH.Web.Tests.Controllers
{
    public class HomeController_Tests: QLKho_NCKHWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}