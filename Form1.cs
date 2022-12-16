namespace WeightVectors
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void GenerateWeights()
        {
            try
            {
                int numObjectives = (int)ObjectivesNum.Value;
                int populationSize = (int)PopulationNum.Value;
                int rounding = (int)RoundingNum.Value;
                string separator = string.IsNullOrEmpty(SepTxt.Text) ? " " : SepTxt.Text;

                if (numObjectives < 1)
                {
                    MessageBox.Show("Error, the number of objectives must be greater than one.");
                    return;
                }
                else if (populationSize < 1)
                {
                    MessageBox.Show("Error, the population size must be greater than one.");
                    return;
                }
                else if (populationSize < numObjectives)
                {
                    MessageBox.Show("Error, the population size must be greater than the number of objectives.");
                    return;
                }

                double[,] weightArray = new double[populationSize, numObjectives];
                Random rand = new Random();

                //1 0 0, 0 1 0, 0 0 1
                //First, give each objective '1' value. 
                for (int i = 0; i < numObjectives; i++)
                    weightArray[i, i] = 1.0d;

                for (int i = numObjectives; i < populationSize; i++)  // the remaining populations 
                {
                    //generate random numbers, and multiply by factor to make sure the sum = 1
                    double[] values = Enumerable.Repeat(0, numObjectives).Select(i => rand.NextDouble()).ToArray();
                    double factor = 1.0d / values.Sum();
                    values = values.Select(v => Math.Round(v * factor, rounding)).ToArray();
                    for (int j = 0; j < numObjectives; j++) 
                        weightArray[i,j] = values[j];
                }

                //Save data to file
                string filename = Directory.GetCurrentDirectory() + @"\W" + numObjectives + "D_" + populationSize + ".txt";
                using (StreamWriter writer = new StreamWriter(filename, false))
                {
                    string row;
                    for (int i = 0; i < weightArray.GetLength(0); ++i)
                    {
                        row = string.Join(separator, Enumerable.Range(0, weightArray.GetLength(1)).Select(j => weightArray[i, j]));
                        writer.WriteLine(row);
                    }
                }
                MessageBox.Show($"The weight vectors are generated successfully.{Environment.NewLine}Output file: {filename}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error, {ex.Message}");
                return;
            }
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            GenerateWeights();
        }
    }
}
