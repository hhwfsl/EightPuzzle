using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.ComponentModel;
namespace EightPuzzle;

public partial class Page_Main : Page,INotifyPropertyChanged
{
    NineGrid operationGrid = new NineGrid();//当前九宫格
    NineGrid targetGrid = new NineGrid();//目标九宫格

    public List<Node> FlatMapL { get; set; }//数据绑定UniformGrid的九个按钮对应Node(当前)
    public List<Node> FlatMapR { get; set; }//数据绑定UniformGrid的九个按钮对应Node(目标)
    Node Blank = new Node();//空格
    private int _stepCount = 0;//完成所用步数
    DateTime startTime = DateTime.Now;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public int StepCount
    {
        get { return _stepCount; }
        set 
        { 
            _stepCount = value;
            OnPropertyChanged(nameof(StepCount));
        }
    }
    public Page_Main()
    {
        InitializeComponent();
        FlatMapL = new List<Node>();
        FlatMapR = new List<Node>();
        InitializeGame(ref operationGrid, ref targetGrid);//初始化游戏
        DataContext = this;//设置DataContext用于数据绑定
    }
    /// <summary>
    /// 初始化游戏
    /// </summary>
    /// <param name="operationGrid">九宫格引用</param>
    /// <param name="targetGrid">目标九宫格引用</param>
    private void InitializeGame(ref NineGrid operationGrid, ref NineGrid targetGrid)
    {
        SetGridValue(ref operationGrid);//随机生成当前九宫格
        SetGridValue(ref targetGrid);//随机生成目标九宫格
        while (!CheckSolvable(operationGrid, targetGrid))//检查逆序数判断有无解
        {
            SetGridValue(ref targetGrid);//无解则重新生成目标九宫格
        }
        InitializeBinding();//初始化数据绑定
        FindBlank();//初始化Blank
        StepCount = 0;//初始化步数
        startTime = DateTime.Now;
    }
    /// <summary>
    /// 在FlatMap中找到空格并用Blank指向它
    /// </summary>
    private void FindBlank()
    {
        for (int i = 0; i < FlatMapL.Count; i++)
        {
            if (FlatMapL[i].Value == 0)
            {
                Blank = FlatMapL[i];
                break;
            }
        }
        operationGrid.BlankX = Blank.X;
        operationGrid.BlankY = Blank.Y;
    }
    /// <summary>
    /// 初始化数据绑定
    /// </summary>
    private void InitializeBinding()
    {
        FlatMapL = new List<Node>();//初始化FlatMapL
        FlatMapR = new List<Node>();//初始化FlatMapR
        for (int i = 0; i < Constants.GridRange; i++)
        {
            for (int j = 0; j < Constants.GridRange; j++)
            {
                FlatMapL.Add(operationGrid.map[i, j]);//逐个加入九宫格
                FlatMapR.Add(targetGrid.map[i, j]);//逐个加入目标九宫格
            }
        }
    }
    /// <summary>
    /// 检查是否有解
    /// </summary>
    /// <param name="operationGrid">九宫格</param>
    /// <param name="targetGrid">目标九宫格</param>
    /// <returns></returns>
    private bool CheckSolvable(NineGrid operationGrid, NineGrid targetGrid)
    {
        if (CheckIsSameGrid(operationGrid, targetGrid))
        {
            return false;
        }
        return operationGrid.inverseNumber % 2 == targetGrid.inverseNumber % 2;
    }
    /// <summary>
    /// 检查是否九宫格和目标九宫格是一样的
    /// </summary>
    /// <param name="operationGrid">九宫格</param>
    /// <param name="targetGrid">目标九宫格</param>
    /// <returns></returns>
    private bool CheckIsSameGrid(NineGrid operationGrid, NineGrid targetGrid)
    {
        return operationGrid.Equals(targetGrid);
    }
    /// <summary>
    /// 设置九宫格值(初始化)
    /// </summary>
    /// <param name="nineGrid">九宫格引用</param>
    private void SetGridValue(ref NineGrid nineGrid)
    {
        List<int> list = Enumerable.Range(0, Constants.GridRange * Constants.GridRange).ToList();
        Random random = new Random();
        list = list.OrderBy(x => random.Next()).ToList();//随机排列
        nineGrid.SetValue(list);//设置值
    }
    /// <summary>
    /// 点击新游戏按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NewGame_Click(object sender, RoutedEventArgs e)
    {
        InitializeGame(ref operationGrid, ref targetGrid);//初始化游戏
    }
    /// <summary>
    /// 点击九宫格按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TileButton_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;//取事件发起按钮
        if (button == null)
        {
            //空对象处理
            MessageBox.Show("TileButton中有Null对象", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        Node block = (Node)button.DataContext;//取其所绑定的Node
        if (block == null)
        {
            //空对象处理
            MessageBox.Show("TileButton中有Null对象", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (CheckMovable(block))//检查是否是可移动格子
        {
            Swap(block);//如果可移动，交换
        }
    }
    /// <summary>
    /// 检查格子是否可以移动
    /// </summary>
    /// <param name="block">目标格子</param>
    /// <returns></returns>
    private bool CheckMovable(Node block)
    {
        if (Math.Abs(block.X - Blank.X) + Math.Abs(block.Y - Blank.Y) == 1)
        {
            return true;//是空格的合法上下左右的格子则返回true
        }
        return false;//否则返回false
    }
    /// <summary>
    /// 交换两个格子
    /// </summary>
    /// <param name="block">目标格子</param>
    private void Swap(Node block)
    {
        ValueSwap(block);//值转移
        FindBlank();//重新确定Blank
        UpdateOperationGrid();//更新九宫格二维数组
        StepCount++;//步数加1
        if (CheckWin())//检查是否已经达到目标九宫格
        {
            WinGame();//如果达到则进入WinGame
        }
    }
    /// <summary>
    /// 写入游戏记录
    /// </summary>
    /// <param name="endTime">结束时间</param>
    /// <param name="duringTime">经历时间</param>
    private void WriteRecord(DateTime endTime, double duringTime)
    {
        if (!Directory.Exists(Constants.DirectoryData))
        {
            Directory.CreateDirectory(Constants.DirectoryData);
        }
        string[] files = Directory.GetFiles(Constants.DirectoryData);
        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open($"./Data/record{files.Length + 1}.gamedata", FileMode.Create)))
        {
            binaryWriter.Write(files.Length);
            binaryWriter.Write(startTime.Ticks);
            binaryWriter.Write(endTime.Ticks);
            binaryWriter.Write(duringTime);
            binaryWriter.Write(StepCount);
        }
    }
    /// <summary>
    /// 达成目标，游戏获胜
    /// </summary>
    private void WinGame()
    {
        DateTime endTime = DateTime.Now;
        double duringTime = Math.Round((endTime - startTime).TotalMinutes, 3);
        Task.Run(() => { WriteRecord(endTime, duringTime); });
        MessageBoxResult result = MessageBox.Show($"你已完成八数码问题的一次求解, 共消耗步数: {StepCount}\n开始时间: {startTime}\n结束时间: {endTime}\n总耗时: {duringTime}分钟\n是否再来一局?", "Win", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            StartNewGame();//开启新一轮
        }
    }
    /// <summary>
    /// 开始新游戏
    /// </summary>
    private void StartNewGame()
    {
        InitializeGame(ref operationGrid, ref targetGrid);//初始化游戏
    }
    /// <summary>
    /// 检查是否获胜
    /// </summary>
    /// <returns></returns>
    private bool CheckWin()
    {
        for (int i = 0; i < FlatMapL.Count; i++)
        {
            if (FlatMapL[i] != null && FlatMapR[i] != null)
            {
                if (!(FlatMapL[i].Value == FlatMapR[i].Value))
                {
                    return false;//当前九宫格和目标九宫格不一样，返回false
                }
            }
            else
            {
                //空对象引用处理
                MessageBox.Show("FlatMap中有Null对象", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        return true;//一样返回true
    }
    /// <summary>
    /// 格子值交换
    /// </summary>
    /// <param name="block"></param>
    private void ValueSwap(Node block)
    {
        int tempValue = Blank.Value;
        int tempIndex = Blank.Index;

        int tempValue2 = block.Value;
        int tempIndex2 = block.Index;

        FlatMapL[tempIndex].Value = tempValue2;

        FlatMapL[block.Index].Value = tempValue;
    }
    /// <summary>
    /// 更新九宫格二维数组
    /// </summary>
    private void UpdateOperationGrid()
    {
        int k = 0;
        for (int i = 0; i < Constants.GridRange; i++)
        {
            for (int j = 0; j < Constants.GridRange; j++)
            {
                operationGrid.map[i, j] = FlatMapL[k++];
            }
        }
    }
    /// <summary>
    /// 打开记录页面
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OpenRecord_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new Page_Record());
    }

    
    
    /*
    private void FakeRecord_Click(object sender, RoutedEventArgs e)
    {
        DateTime endTime = DateTime.Now;
        double duringTime = Math.Round((endTime - startTime).TotalMinutes, 3);
        Task.Run(() => { WriteRecord(endTime, duringTime); });
    }
    */
    /*
    private void ClearRecords()
    {
        if (!Directory.Exists(Constants.DirectoryData))
        {
            Directory.CreateDirectory(Constants.DirectoryData);
        }
        string[] files = Directory.GetFiles(Constants.DirectoryData);
        List<Record> records = new List<Record>();
        for (int i = 1; i <= files.Length; i++)
        {
            File.Delete($"./Data/record{i}.gamedata");
        }
    }

    private void ClearRecord_Click(object sender, RoutedEventArgs e)
    {
        ClearRecords();
    }
    */
}