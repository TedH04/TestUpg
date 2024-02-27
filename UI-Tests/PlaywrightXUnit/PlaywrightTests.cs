using Microsoft.Playwright;

namespace PlaywrightXUnit
{
    public class PlaywrightTests : IAsyncLifetime
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IBrowserContext? _context;
        private IPage? _page;

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            var edgeExecutablePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                ExecutablePath = edgeExecutablePath
            });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser.DisposeAsync();
            }
            if (_playwright != null)
            {
                _playwright.Dispose();
            }
        }

        [Fact]
        public async Task TestGameFunctionality()
        {
            Console.WriteLine("------------TEST HAS BEGUN------------");

            await _page.GotoAsync("http://localhost:3000/");

            // startar spelet
            await _page.WaitForTimeoutAsync(1000);
            await _page.ClickAsync("#startgame-button");
            await _page.WaitForSelectorAsync("#checkinventory-button");
            Console.WriteLine("Clicked start game button");

            // kollar attack power
            var startAttackPower = await _page.InnerTextAsync("#attackpower-tag");
            Console.WriteLine("Attack power before equip: " + startAttackPower + "'.");

            // kollar inv och equip svärd
            await _page.WaitForTimeoutAsync(1000);
            await _page.ClickAsync("#checkinventory-button");
            await _page.WaitForTimeoutAsync(1000);
            await _page.WaitForSelectorAsync("#equip-Sword");
            Console.WriteLine("Clicked check inventory button");

            await _page.ClickAsync("#equip-Sword");
            await _page.WaitForTimeoutAsync(1000);
            Console.WriteLine("Clicked equip button");

            var attackPower = await _page.InnerTextAsync("#attackpower-tag");
            Assert.Equal("Attack Power: 3", attackPower);

            Console.WriteLine("Test passed: Attack power has increased to 3");
            Console.WriteLine("------------TEST CONTINUES WITH BATTLE------------");

            // Simulate battles
            await _page.WaitForTimeoutAsync(1000);
            for (int i = 0; i < 5; i++)
            {
                await _page.ClickAsync("#startbattle-button");
                await _page.WaitForTimeoutAsync(1000);
                Console.WriteLine("Battle started");

                var enemyType = await _page.InnerTextAsync("#enemy-type-name");

                await _page.ClickAsync("#attack-button");
                await _page.WaitForTimeoutAsync(1000);
                Console.WriteLine("Clicked attack button");

                while (await _page.IsVisibleAsync("#attack-button"))
                {
                    await _page.ClickAsync("#attack-button");
                    await _page.WaitForTimeoutAsync(1000);
                    Console.WriteLine("Clicked attack button");
                }

                Console.WriteLine($"{enemyType} has been slain");
                await _page.WaitForTimeoutAsync(1000);
                var myMoney = await _page.InnerTextAsync("#money-tag");
                Console.WriteLine(myMoney);
            }

            Console.WriteLine("------------TEST CONTINUES WITH STORE------------");

            await _page.ClickAsync("#enterStore-button");
            await _page.WaitForTimeoutAsync(1000);
            Console.WriteLine("Clicked enter store button");

            await _page.ClickAsync("#buy-Shield");
            await _page.WaitForTimeoutAsync(1000);
            Console.WriteLine("Item successfully bought from store");

            await _page.ClickAsync("#leave-button");
            await _page.WaitForTimeoutAsync(1000);
            Console.WriteLine("Left store and went to town");

            // kollar inv och equip breastplate
            await _page.WaitForTimeoutAsync(1000);
            await _page.ClickAsync("#checkinventory-button");
            await _page.WaitForTimeoutAsync(1000);
            await _page.WaitForSelectorAsync("#equip-Shield");
            Console.WriteLine("Clicked check inventory button");

            await _page.ClickAsync("#equip-Shield");
            await _page.WaitForTimeoutAsync(1000);
            Console.WriteLine("Clicked equip button");

            var myMoneyAfterPurchase = await _page.InnerTextAsync("#money-tag");
            Console.WriteLine("Opens wallet and checks money");
            Console.WriteLine(myMoneyAfterPurchase);
            await _page.WaitForTimeoutAsync(5000);
        }
    }

}