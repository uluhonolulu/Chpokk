using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using CThru;
using CThru.BuiltInAspects;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.CustomerDevelopment.Notification;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Simple.Data;
using Shouldly;
using Simple.Data.Ado;
using TypeMock.ArrangeActAssert;

namespace Chpokk.Tests.NewSubscription {
	[TestFixture]
	public class Notification: BaseCommandTest<UserInTheDB> {
		private readonly DateTime _monthLater = DateTime.Parse("2014-02-01");

		[Test]
		public void PaidUntilShouldBeInAMonth() {
			var user = Context.GetUser();
			Assert.IsNotNull(user);
			((DateTime) user.PaidUntil).ShouldBe(_monthLater);
		}

		public override void Act() {
			var notificationEndpoint = Context.Container.Get<NotificationEndpoint>();
			notificationEndpoint.UpdateSubscriptionStatus(new NotificationInputModel{SubscriptionReferrer = Context.UserName});
		}
	}

	public class UserInTheDB: SimpleConfiguredContext {
		public string UserName = Guid.NewGuid().ToString();
		private DateTime today = DateTime.Parse("2014-01-01");
		public override void Create() {
			base.Create();
			var db = Database.Open();
			db.Users.Insert(new {UserId = UserName, Data = String.Empty});

			Isolate.WhenCalled(() => DateTime.Now).WillReturn(today);

		}

		public dynamic GetUser() {
			var db = Database.Open();
			return db.Users.FindByUserId(UserName);
		}

		public override void Dispose() {
			base.Dispose();
			var db = Database.Open();
			//Console.WriteLine();
			//Console.WriteLine("Deleting a user");
			db.Users.DeleteByUserId(UserName);
		}
	}
}
