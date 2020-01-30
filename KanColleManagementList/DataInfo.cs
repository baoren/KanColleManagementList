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
                CSVinf.ReadCSV(KanColleData, false, CSVinf.KanColleCsvFilePath, ",", false);
            }
            return KanColleData;
        }

        private String BattleShipModel(String BattleShipType)
        {
            String Type = "";
            switch ( BattleShipType ) 
            {
                case "戦艦":
                case "巡洋戦艦":
                case "航空戦艦":
                case "超弩級戦艦":
                    Type = "戦艦";
                break;
                case "空母":
                case "装甲空母":
                case "軽空母":
                    Type = "空母";
                break;
                case "重巡洋艦":
                case "航空巡洋艦":
                    Type = "重巡洋艦";
                break;
                case "軽巡洋艦":
                case "重雷装巡洋艦":
                case "練習巡洋艦":
                case "防空巡洋艦":
                    Type = "軽巡洋艦";
                break;
                case "駆逐艦":
                    Type = "駆逐艦";
                break;
                case "海防艦":
                    Type = "海防艦";
                break;
                case "潜水艦":
                case "潜水母艦":
                    Type = "潜水艦";
                break;
                default:
                    Type = "その他";
                break;
            }
            return Type;
        }

    }
}
