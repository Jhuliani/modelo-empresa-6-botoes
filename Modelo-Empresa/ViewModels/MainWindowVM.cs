using Modelo_Empresa.DataBase;
using Modelo_Empresa.Models;
using Modelo_Empresa.Services;
using Modelo_Empresa.Views;
using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Modelo_Empresa.ViewModels
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private IDataBase _connection;

        public ObservableCollection<FuncionarioModel> listaFuncionarios { get; set; }
        public ObservableCollection<ProjetoModel> listaProjetos { get; set; }

        public ICommand AdicionarFuncionario { get; private set; }
        public ICommand RemoverFuncionario { get; private set; }
        public ICommand AtualizarFuncionario { get; private set; }
        public ICommand AdicionarProjeto { get; private set; }
        public ICommand RemoverProjeto { get; private set; }
        public ICommand AtualizarProjeto { get; private set; }

        public FuncionarioModel FuncionarioSelecionado { get; set; }
        public ProjetoModel ProjetoSelecionado { get; set; }
        public Opcoes OpcaoSelecionada { get; set; }


        public MainWindowVM()
        {
            _connection = new PostgresDb();
            listaFuncionarios = new ObservableCollection<FuncionarioModel>(_connection.ListarFuncionarios());
            listaProjetos = new ObservableCollection<ProjetoModel>(_connection.ListarProjetos());
            InicializarFuncionarioCommands();
            InicializarProjetosCommands();
        }




        public void InicializarFuncionarioCommands()
        {
            AdicionarFuncionario = new RelayCommand((object _) =>
            {
                FuncionarioModel novoFuncionario = new FuncionarioModel();
                FuncionarioV projetoWindow = new FuncionarioV();
                projetoWindow.DataContext = novoFuncionario;
                bool? resultadoDialog = projetoWindow.ShowDialog();
                if (resultadoDialog.HasValue && resultadoDialog.Value == true)
                {
                    try
                    {
                        _connection.AdicionarFuncionario(novoFuncionario);
                        listaFuncionarios.Clear();
                        listaFuncionarios = new ObservableCollection<FuncionarioModel>(_connection.ListarFuncionarios());
                        Notifica(nameof(listaFuncionarios));
                        MessageBox.Show("Funcionário inserido");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao inserir Funcionário \n"
                        + ex.Message);
                    }
                }
            });

            RemoverFuncionario = new RelayCommand((object _) =>
            {
                if (FuncionarioSelecionado != null)
                {
                    try
                    {
                        _connection.RemoverFuncionario(FuncionarioSelecionado);
                        listaFuncionarios.Clear();
                        listaFuncionarios = new ObservableCollection<FuncionarioModel>(_connection.ListarFuncionarios());
                        Notifica(nameof(listaFuncionarios));
                        MessageBox.Show("Funcionário excluído!!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao deletar Funcionário \n"
                            + ex.Message);
                    }
                }
            });

            AtualizarFuncionario = new RelayCommand((object _) =>
            {
                FuncionarioV funcionarioWindow = new FuncionarioV();
                funcionarioWindow.DataContext = FuncionarioSelecionado;
                bool? resultadoDialog = funcionarioWindow.ShowDialog();
                if (resultadoDialog.HasValue && resultadoDialog.Value == true)
                {
                    try
                    {
                        _connection.AtualizarFuncionario(FuncionarioSelecionado);
                        listaFuncionarios.Clear();
                        listaFuncionarios = new ObservableCollection<FuncionarioModel>(_connection.ListarFuncionarios());
                        Notifica(nameof(listaFuncionarios));
                        MessageBox.Show("Funcionário atualizado!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao atualizar Funcionário \n"
                            + ex.Message);
                    }
                }
            });
        }

        public void InicializarProjetosCommands()
        {
            AdicionarProjeto = new RelayCommand((object _) =>
            {
                ProjetoModel novoProjeto = new ProjetoModel();
                ProjetoV projetoWindow = new ProjetoV();
                projetoWindow.DataContext = novoProjeto;
                bool? resultadoDialog = projetoWindow.ShowDialog();
                if (resultadoDialog.HasValue && resultadoDialog.Value == true)
                {
                    try
                    {
                        _connection.AdicionarProjeto(novoProjeto);
                        listaProjetos.Clear();
                        listaProjetos = new ObservableCollection<ProjetoModel>(_connection.ListarProjetos());
                        Notifica(nameof(listaProjetos));
                        MessageBox.Show("Projeto inserido");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao inserir Projeto \n"
                        + ex.Message);
                    }
                }
            });

            RemoverProjeto = new RelayCommand((object _) =>
            {
                if (ProjetoSelecionado != null)
                {
                    try
                    {
                        _connection.RemoverProjeto(ProjetoSelecionado);
                        listaProjetos.Clear();
                        listaProjetos = new ObservableCollection<ProjetoModel>(_connection.ListarProjetos());
                        Notifica(nameof(listaProjetos));
                        MessageBox.Show("Projeto excluído!!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao deletar Projeto \n"
                            + ex.Message);
                    }
                }
            });

            AtualizarProjeto = new RelayCommand((object _) =>
            {
                ProjetoV projetoWindow = new ProjetoV();
                projetoWindow.DataContext = ProjetoSelecionado;
                bool? resultadoDialog = projetoWindow.ShowDialog();
                if (resultadoDialog.HasValue && resultadoDialog.Value == true)
                {
                    try
                    {
                        _connection.AtualizarProjeto(ProjetoSelecionado);
                        listaProjetos.Clear();
                        listaProjetos = new ObservableCollection<ProjetoModel>(_connection.ListarProjetos());
                        Notifica(nameof(listaProjetos));
                        MessageBox.Show("Projeto atualizado!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao atualizar Projeto \n"
                            + ex.Message);
                    }
                }
            });

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notifica(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

