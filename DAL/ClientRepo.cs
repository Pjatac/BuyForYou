using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class ClientRepo
	{
		public IEnumerable<Client> GetAllClients()
		{
			using (var dbContext = new BFUContext())
			{
				return dbContext.Clients.ToList();
			}
		}
		public void AddClient(Client client)
		{
			using (var dbContext = new BFUContext())
			{
				dbContext.Clients.Add(client);
				dbContext.SaveChanges();
			}
		}
		public void ChangeClient(Client client)
		{
			using (var dbContext = new BFUContext())
			{
				Client cl = FindClient(client.Id);
				cl = client;
				dbContext.Entry(cl).State = EntityState.Modified;
				dbContext.SaveChanges();
			}
		}
		public Client FindClient(string name, string pass)
		{
			using (var dbContext = new BFUContext())
			{
				foreach (var c in dbContext.Clients)
				{
					if (c.UserName == name && c.Pass == pass)
					{
						return c;
					}
				}
				return null;
			}
		}
		public Client FindClient(string name)
		{
			using (var dbContext = new BFUContext())
			{
				foreach (var c in dbContext.Clients)
				{
					if (c.UserName == name)
					{
						return c;
					}
				}
				return null;
			}
		}
		public Client FindClient(long id)
		{
			using (var dbContext = new BFUContext())
			{
				foreach (var c in dbContext.Clients)
				{
					if (c.Id == id)
					{
						return c;
					}
				}
				return null;
			}
		}
		public void DeleteAllClients()
		{
			using (var dbContext = new BFUContext())
			{
				foreach (var c in dbContext.Clients)
				{
					dbContext.Clients.Remove(c);
				}
				dbContext.SaveChanges();
			}
		}
		public void DeleteClient(int id)
		{
			using (var dbContext = new BFUContext())
			{
				foreach (var c in dbContext.Clients)
				{
					if (c.Id == id)
						dbContext.Clients.Remove(c);
				}
				dbContext.SaveChanges();
			}
		}
		public bool CheckUsername(string name)
		{
			using (var dbContext = new BFUContext())
			{
				foreach (var c in dbContext.Clients)
				{
					if (c.UserName == name)
						return true;
				}
			}
			return false;
		}
	}
}
