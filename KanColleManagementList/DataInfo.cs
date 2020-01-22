using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace KanColleManagementList
{
    class DataInfo
    {
        /// <summary>
        /// CSVファイルの操作
        /// </summary>
        CSVInformation CSVinf = new CSVInformation();

        /// <summary>
        /// 艦隊一覧を格納する変数
        /// </summary>
        private DataTable KanColleData = new DataTable("KabColleTable");

        /// <summary>
        /// csvを読み込んだデータテーブルを返す
        /// </summary>
        /// <returns></returns>
        public DataTable inputDatable()
        {
            if (File.Exists(CSVinf.KanColleCsvFilePath))
            {
                CSVinf.ReadCSV(KanColleData, true, CSVinf.KanColleCsvFilePath, ",", false);
            }
            return KanColleData;
        }

    }
}
