using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;


namespace EightPuzzle
{
    public partial class Page_Record : Page
    {
        public Page_Record()
        {
            InitializeComponent();
            ShowRecords();
        }
        /// <summary>
        /// 展示记录
        /// </summary>
        private void ShowRecords()
        {
            RichTextBox_Records.Document.Blocks.Clear();
            List<Record> records = GetRecords();
            foreach (Record record in records)
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add($"RecordID: {record.Id}\n");
                paragraph.Inlines.Add($"StartTime: {record.startTime}\n");
                paragraph.Inlines.Add($"EndTime: {record.endTime}\n");
                paragraph.Inlines.Add($"DuringTime: {record.duringTime} 分\n");
                paragraph.Inlines.Add($"Steps: {record.steps}");
                RichTextBox_Records.Document.Blocks.Add(paragraph);
            }
        }
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <returns></returns>
        private List<Record> GetRecords()
        {
            if (!Directory.Exists(Constants.DirectoryData))
            {
                Directory.CreateDirectory(Constants.DirectoryData);
            }
            string[] files = Directory.GetFiles(Constants.DirectoryData);
            List<Record> records = new List<Record>();
            for(int i = 1;i<=files.Length;i++)
            {
                using (BinaryReader binaryReader = new BinaryReader(File.Open($"./Data/record{i}.gamedata", FileMode.Open)))
                {
                    int ID = binaryReader.ReadInt32();
                    DateTime startTime = new DateTime(binaryReader.ReadInt64());
                    DateTime endTime = new DateTime(binaryReader.ReadInt64());
                    double duringTime = binaryReader.ReadDouble();
                    int steps = binaryReader.ReadInt32();
                    Record record = new Record(ID, startTime, endTime, duringTime, steps);
                    records.Add(record);
                }
            }
            return records;
        }
        /// <summary>
        /// 刷新记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Refresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RichTextBox_Records.Document.Blocks.Clear();
            ShowRecords();
        }
        /// <summary>
        /// 回到主页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        /// <summary>
        /// 清空记录
        /// </summary>
        private void ClearRecords()
        {
            if (!Directory.Exists(Constants.DirectoryData))
            {
                Directory.CreateDirectory(Constants.DirectoryData);
            }
            string[] files = Directory.GetFiles(Constants.DirectoryData);
            for (int i = 1; i <= files.Length; i++)
            {
                File.Delete($"./Data/record{i}.gamedata");
            }
        }
        /// <summary>
        /// 清空记录按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearRecord_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ClearRecords();
            ShowRecords();
        }
    }
}
