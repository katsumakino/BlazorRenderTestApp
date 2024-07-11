using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace BlazorRenderTestApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DBDataController : ControllerBase {
    // Postgre SQL Serverに接続して、Dataを取得する

    public List<DBData> GetDBData() {

        // appsettings.jsonと接続
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        // appsettings.jsonからConnectionString情報取得
        string? ConnectionString = configuration.GetConnectionString("db");

        // 実行するクエリコマンド定義
        string Query = "SELECT * FROM test_list ORDER BY data_id";
        NpgsqlConnection sqlConnection = new(ConnectionString);
        sqlConnection.Open();
        //Using NpgsqlCommand and Query create connection with database
        NpgsqlCommand Command = new(Query, sqlConnection);
        //Using NpgsqlDataAdapter execute the NpgsqlCommand 
        NpgsqlDataAdapter DataAdapter = new(Command);
        DataTable DataTable = new();
        // Using NpgsqlDataAdapter, process the query string and fill the data into the dataset
        DataAdapter.Fill(DataTable);
        sqlConnection.Close();
        // Cast the data fetched from NpgsqlDataAdapter to List<T>
        var DataSource = (from DataRow Data in DataTable.Rows
                          select new DBData() {
                              DataID = Convert.ToInt32(Data["data_id"]),
                              DataName = Data["data_name"].ToString()
                          }).ToList();
        return DataSource;
    }

}

public class DBData {
    public int? DataID { get; set; }
    public string? DataName { get; set; }
}
