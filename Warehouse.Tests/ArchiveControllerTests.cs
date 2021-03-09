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
        [Fact]
        public async void DeleteFromArchive_return_True_Or_False()
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

            ArchiveController archiveController = new ArchiveController(GetContext());
            await archiveController.DeleteFromArchive(TestRecord.ItemID);

            Assert.Equal("Склад не указан", TestRecord.ItemStorageID);
        }


    }
}
