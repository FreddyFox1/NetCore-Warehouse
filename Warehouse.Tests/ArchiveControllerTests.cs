using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Controllers;
using Warehouse.Model;
using Xunit;

namespace Warehouse.Tests
{
    public class ArchiveControllerTests : IntegrationTest
    {
        /// <summary>
        /// Тест для ArchiveController
        /// </summary>
        [Fact]
        public async void DeleteFromArchive_return_True()
        {
            var context = GetContext();
            ItemCategory category = new ItemCategory()
            {
                CategoryName = "TestName"
            };

            context.Add(category);
            context.SaveChanges();

            Item TestRecord = new Item()
            {
                ItemStorageID = "Архив",
                ItemName = "Test",
                ItemArticle = "123-123-123-123",
                CategoryID = category.CategoryID
            };

            context.Add(TestRecord);
            context.SaveChanges();

            var expected = TestRecord.ItemStorageID;

            ArchiveController archiveController = new ArchiveController(context);
            await archiveController.DeleteFromArchive(TestRecord.ItemID);

            var actual = context.Items.FirstOrDefault(a => 
            a.ItemName == TestRecord.ItemName).ItemStorageID;
           
            Assert.DoesNotMatch(expected, actual);
        }


    }
}
