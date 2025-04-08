
namespace EightPuzzle
{
    /// <summary>
    /// NineGrid九宫格类，用于管理九宫格
    /// </summary>
    class NineGrid
    {
        public Node[,] map = new Node[Constants.GridRange,Constants.GridRange];//九宫格二维数组
        public int inverseNumber = 0;//逆序数
        int _blankX = 0;
        int _blankY = 0;
        
        public int BlankX
        {
            get { return _blankX; }
            set
            {
                _blankX = value;
            }
        }
        public int BlankY
        {
            get { return _blankY; }
            set
            {
                _blankY = value;
            }
        }
        /// <summary>
        /// 无参构造函数，初始化九宫格
        /// </summary>
        public NineGrid()
        {
            for(int i = 0; i < Constants.GridRange; i++)
            {
                for (int j = 0; j < Constants.GridRange; j++)
                {
                    map[i,j] = new Node();
                }
            }
        }
        public NineGrid(NineGrid copyMap)
        {
            for (int i = 0; i < Constants.GridRange; i++)
            {
                for (int j = 0; j < Constants.GridRange; j++)
                {
                    map[i, j] = new Node(copyMap.map[i, j]);
                }
            }
            BlankX = copyMap.BlankX;
            BlankY = copyMap.BlankY;
        }
        /// <summary>
        /// 计算逆序数
        /// </summary>
        /// <param name="list">数值排列表</param>
        private void SetInverseNumber(List<int> list)
        {
            int count = 0;
            for(int i=0;i<list.Count-1;i++)
            {
                int temp = list[i];
                for(int j = i+1;j<list.Count;j++)
                {
                    if(temp!=0 && list[j]!=0 && temp > list[j])
                    {
                        count++;
                    }
                }
            }
            inverseNumber = count;
        }
        /// <summary>
        /// 设置每格相关数据
        /// </summary>
        /// <param name="list">数值排列表</param>
        public void SetValue(List<int> list)
        {
            int k = 0;
            for(int i = 0;i<Constants.GridRange;i++)
            {
                for(int j = 0;j<Constants.GridRange;j++)
                {
                    map[i,j].Value = list[k];
                    if (list[k] == 0)
                    {
                        map[i, j].DisplayValue = " ";
                    }
                    else
                    {
                        map[i, j].DisplayValue = list[k].ToString();
                    }
                    map[i,j].Index = k;
                    map[i, j].X = k / Constants.GridRange;
                    map[i, j].Y = k % Constants.GridRange;
                    k++;
                }
            }
            SetInverseNumber(list);
        }
        /// <summary>
        /// 设置九宫格Node值
        /// </summary>
        /// <param name="list">Node排列表</param>
        public void SetValue(List<Node> list)
        {
            int k = 0;
            for (int i = 0; i < Constants.GridRange; i++)
            {
                for (int j = 0; j < Constants.GridRange; j++)
                {
                    map[i,j] = new Node(list[k++]);
                }
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is NineGrid other)
            {
                for (int i = 0; i < Constants.GridRange; i++)
                {
                    for (int j = 0; j < Constants.GridRange; j++)
                    {
                        if (map[i, j].Value != other.map[i, j].Value)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < Constants.GridRange; i++)
            {
                for (int j = 0; j < Constants.GridRange; j++)
                {
                    hash = hash * 3 + map[i, j].Value.GetHashCode();
                }
            }
            return hash;
        }
    }
}
