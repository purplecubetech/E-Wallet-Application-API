using E_Wallet_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_WalletApp.DB.Context
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {
        }
        public DbSet<User> users { get; set; }
        public DbSet<Wallet> wallets { get; set; }
        public DbSet<Transaction> transactions { get; set; }
    }
}
