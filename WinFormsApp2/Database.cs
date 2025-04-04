﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp2;

public static class Database
{
    private static readonly string stringDeConexao = "Server=localhost;Port=3306;User Id=root; database=ti_113_windowsforms;";

    public static bool SalvarUsuario(Usuario usuario)
    {
        using (MySqlConnection conexao = new MySqlConnection(stringDeConexao))
        {
            conexao.Open();

            string verificaQuery = "SELECT COUNT(*) FROM usuarios WHERE Telefone = @Telefone";
            using (MySqlCommand verificaCmd = new MySqlCommand(verificaQuery, conexao))
            {
                verificaCmd.Parameters.AddWithValue("@Telefone", usuario.Telefone);
                int count = Convert.ToInt32(verificaCmd.ExecuteScalar());

                if (count > 0)
                {
                    return false;
                }
            }

            string query = "INSERT INTO usuarios (Id, Nome, Telefone) VALUES (@Id, @Nome, @Telefone)";
            using (MySqlCommand cmd = new MySqlCommand(query, conexao))
            {
                cmd.Parameters.AddWithValue("@Id", usuario.Id);
                cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@Telefone", usuario.Telefone);

                int quantidade = cmd.ExecuteNonQuery();
                return quantidade > 0;
            }
        }
    }

    public static List<Usuario> GetAlunos()
    {
        List<Usuario> usuarios = new List<Usuario>();

        using (MySqlConnection conexao = new MySqlConnection(stringDeConexao))
        {
            conexao.Open();
            string query = "SELECT Id, Nome, Telefone FROM alunos";

            using (MySqlCommand cmd = new MySqlCommand(query, conexao))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nome = reader.GetString("Nome"),
                        Telefone = reader.GetString("Telefone")
                    });
                }
            }
        }

        return usuarios;
    }
}