using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Model;
using Warehouse.Services.Bitrix24Service;
using Xunit;

namespace Warehouse.Tests
{
    public class BitrixServiceTests : IntegrationTest
    {

        Item item = new Item()
        {
            ItemID = 1,
            ItemName = "TestItem",
            ItemArticle = "ZMF-130-222-333"
        };

        [Fact]
        public void CreateTaskTest_return_string_with_article()
        {
            BitrixService bitrixService;
            bitrixService = new BitrixService(GetBitrixKeysOptions(),
                                              GetLoggerBitrix());
            var expected = item.ItemArticle;
            var actual = bitrixService.CreateTask(item);
            Assert.Contains(expected, actual);
        }

        [Fact]
        public void PushTask_return_True()
        {
            BitrixService bitrixService = new BitrixService(GetBitrixKeysOptions(),
                                                            GetLoggerBitrix());
            var str = bitrixService.CreateTask(item);
            var actual = bitrixService.PushTask(str, item.ItemArticle);

            Assert.True(actual);
        }
    }
}
