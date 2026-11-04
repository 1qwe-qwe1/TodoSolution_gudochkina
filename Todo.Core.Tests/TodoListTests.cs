namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Add_IncreasesCount()
        {
            var list = new TodoList();
            list.Add(" task ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items.First().Title);
        }

        [Fact]
        public void Remove_ById_Works()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void Find_ReturnsMatches()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var found = list.Find("buy").ToList();
            Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }

        [Fact]
        public void Save_And_Load_RestoresItems()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            list.Items.First().MarkDone();

            var tempFile = Path.GetTempFileName();

            try
            {
                list.Save(tempFile);

                var newList = new TodoList();
                newList.Load(tempFile);

                Assert.Equal(2, newList.Count);
                Assert.Equal("Buy milk", newList.Items[0].Title);
                Assert.True(newList.Items[0].IsDone);
                Assert.Equal("Read book", newList.Items[1].Title);
                Assert.False(newList.Items[1].IsDone);
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }

        [Fact]
        public void Load_NonExistentFile_ThrowsException()
        {
            var list = new TodoList();
            Assert.Throws<FileNotFoundException>(() => list.Load("non_existent_file.json"));
        }
    }
}