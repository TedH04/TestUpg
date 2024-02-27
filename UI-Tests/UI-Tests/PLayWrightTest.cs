using Microsoft.Playwright;

namespace UI_Tests
{
    public class PLayWrightTest
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("------------TEST HAS BEGUN------------");

                // startar playwright
                using var playwright = await Playwright.CreateAsync();

                // detta är bara för att använda microsoft edge (funkar bättre enligt mig)
                var edgeExecutablePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";

                await using var browser = await playwright.Chromium.LaunchAsync(new()
                {
                    Headless = false,
                    ExecutablePath = edgeExecutablePath
                });

                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();

                await page.GotoAsync("http://localhost:3000/");

                // klickar på starta spelet
                await page.ClickAsync("#startgame-button");
                await page.WaitForNavigationAsync();
                Console.WriteLine("Clicked start game button");

                // skriver ut attack power innan svärdet är på
                var startattackpower = await page.InnerTextAsync("#attackpower-tag");
                Console.WriteLine("Attack power before equip: " + startattackpower + "'.");

                // kollar inventory
                await page.ClickAsync("#checkinventory-button");
                await page.WaitForSelectorAsync("#equip-button");
                Console.WriteLine("Clicked check inventory button");

                // sätter på svärdet
                await page.ClickAsync("#equip-button");
                await page.WaitForTimeoutAsync(1000);
                Console.WriteLine("Clicked equip button");

                var attackPower = await page.InnerTextAsync("#attackpower-tag");
                if (attackPower != "Attack Power: 3")
                {
                    Console.WriteLine("Test failed: Expected attack power to be '3', but found '" + attackPower + "'.");
                    return;
                }
                else
                {
                    Console.WriteLine("Test passed: Attack power has increased to 3");
                }
                Console.WriteLine("------------TEST CONTINUES WITH BATTLE------------");


                await page.WaitForTimeoutAsync(1000);
                for (int i = 0; i < 4; i++)
                {
                    // Startar en battle
                    await page.ClickAsync("#startbattle-button");
                    await page.WaitForTimeoutAsync(1000);
                    Console.WriteLine("Battle started");

                    //Hämtar ut vad för sorts enemy det är
                    var enemyType = await page.InnerTextAsync("#enemy-type-name");

                    // klickar på attack
                    await page.ClickAsync("#attack-button");
                    await page.WaitForTimeoutAsync(1000);
                    Console.WriteLine("Clicked attack button");

                    while (true)
                    {
                        try
                        {
                            await page.WaitForSelectorAsync("#attack-button");
                            await page.WaitForTimeoutAsync(1000);
                            await page.ClickAsync("#attack-button");
                            await page.WaitForTimeoutAsync(1000);

                            Console.WriteLine("Clicked attack button");
                        }
                        catch
                        {
                            break;
                        }
                    }

                    Console.WriteLine($"{enemyType} has been slain");
                    await page.WaitForTimeoutAsync(1000);
                    var myMoney = await page.InnerTextAsync("#money-tag");
                    Console.WriteLine(myMoney);
                }

                Console.WriteLine("------------TEST CONTINUES WITH STORE------------");

                await page.ClickAsync("#enterStore-button");
                await page.WaitForTimeoutAsync(1000);
                Console.WriteLine("Clicked enter store button");

                await page.ClickAsync("#buy-button");
                await page.WaitForTimeoutAsync(1000);
                Console.WriteLine("item successfully bought from store");

                await page.ClickAsync("#leave-button");
                await page.WaitForTimeoutAsync(1000);
                Console.WriteLine("left store and went to town");

                var myMoneyAfterPurchase = await page.InnerTextAsync("#money-tag");
                Console.WriteLine("opens wallet and checks money");
                Console.WriteLine(myMoneyAfterPurchase);

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            finally
            {
                Console.WriteLine("Test completed successfully!");
                Console.ReadLine();
            }
        }
    }
}
