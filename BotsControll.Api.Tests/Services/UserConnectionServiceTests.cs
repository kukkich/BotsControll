using BotsControll.Api.Services.Users;
using BotsControll.Api.Web.Connections;
using BotsControll.Core.Identity;

namespace BotsControll.Api.Tests.Services;

public class UserConnectionServiceTests
{
    private UserConnectionService _connectionService;
    private UserConnectionRepository _connectionRepository;

    [SetUp]
    public void Setup()
    {
        _connectionRepository = new UserConnectionRepository();
        _connectionService = new UserConnectionService(_connectionRepository);
    }

    [Test]
    public void GivenOneUser_WhenDisconnectHim_ThenEmptyRepositoryExpected()
    {
        var user = new UserIdentity(1, "123");
        _connectionService.Connect(user, "connection_id");

        _connectionService.Disconnect(user, "connection_id", null);
        
        Assert.That(_connectionRepository.GetAll().Any(), Is.EqualTo(false));
    }

    [Test]
    public void GivenOneUser_ThenOneUserWithOneConnectionExpected()
    {
        var user = new UserIdentity(1, "123");
        _connectionService.Connect(user, "connection_id");

        var lengthActual = _connectionRepository.GetAll().Count();
        var (key, connectedUser) = _connectionRepository.GetAll().First();

        const int lengthExpected = 1;


        Assert.Multiple(() =>
        {
            Assert.That(lengthActual, Is.EqualTo(lengthExpected));
            Assert.That(key, Is.EqualTo(user.Id));
            Assert.That(connectedUser.User.Id, Is.EqualTo(1));
            Assert.That(connectedUser.User.Name, Is.EqualTo("123"));
            Assert.That(connectedUser.ConnectionIds, Is.EquivalentTo(new [] { "connection_id" }));
        });
    }

    [Test]
    public void GivenOneUserWithTwoConnections_WhenHeConnectedTwice_ThenOneUserWithTwoConnectionsExpected()
    {
        var user = new UserIdentity(1337, "user_name");
        _connectionService.Connect(user, "connection_id_one");
        _connectionService.Connect(user, "connection_id_two");

        var lengthActual = _connectionRepository.GetAll().Count();
        var (key, connectedUser) = _connectionRepository.GetAll().First();

        const int lengthExpected = 1;
        var expectedConnections = new[]
        {
            "connection_id_one",
            "connection_id_two"
        }.Order();

        Assert.Multiple(() =>
        {
            Assert.That(lengthActual, Is.EqualTo(lengthExpected));
            Assert.That(key, Is.EqualTo(user.Id));
            Assert.That(connectedUser.User.Id, Is.EqualTo(1337));
            Assert.That(connectedUser.User.Name, Is.EqualTo("user_name"));
            Assert.That(connectedUser.ConnectionIds.Order(), Is.EquivalentTo(expectedConnections));
        });
    }

    [Test]
    public void GivenOneUserWithTwoConnections_WhenHeDisconnectedConnectedTwice_ThenNoUserConnectionsExpected()
    {
        var user = new UserIdentity(1337, "user_name");

        _connectionService.Connect(user, "connection_id_one");
        _connectionService.Connect(user, "connection_id_two");
        
        _connectionService.Disconnect(user, "connection_id_one");
        _connectionService.Disconnect(user, "connection_id_two");


        Assert.That(_connectionRepository.GetAll().Any(), Is.EqualTo(false));
    }

    [TestCase("forDisconnection", "1", "forDisconnection", "3", "10")]
    [TestCase("forDisconnection", "forDisconnection", "1", "3", "10")]
    public void GivenUserWithManyConnections_WhenOneConnectionIsRemoved_ThenUserWithoutRemovedConnectionExpected(
        string idForDisconnect,
        params string[] connectionIds
        )
    {
        var user = new UserIdentity(12, "user_name");
        foreach (var connectionId in connectionIds)
        {
            _connectionService.Connect(user, connectionId);
        }
        _connectionService.Disconnect(user, idForDisconnect);

        var (_, connectedUser) = _connectionRepository.GetAll().First();
        var connectionsActual = connectedUser.ConnectionIds.Order();
        var expectedConnections = connectionIds.Order().Where(x => x != idForDisconnect);

        Assert.That(connectionsActual, Is.EquivalentTo(expectedConnections));
    }

    [Test]
    public void GivenTwoUsersWithManyConnections_WhenManyConnectionIsRemoved_ThenUsersWithoutRemovedConnectionExpected()
    {
        var user1 = new UserIdentity(12, "user_name1");
        var user2 = new UserIdentity(51, "user_name2");

        _connectionService.Connect(user1, "1_1");
        _connectionService.Connect(user1, "1_2");
        _connectionService.Connect(user1, "1_3");

        _connectionService.Connect(user2, "2_1");
        _connectionService.Connect(user2, "2_2");
        _connectionService.Connect(user2, "2_3");
        _connectionService.Connect(user2, "2_4");
        _connectionService.Connect(user2, "2_5");

        _connectionService.Disconnect(user1, "1_2");
        _connectionService.Disconnect(user2, "2_4");
        _connectionService.Disconnect(user2, "2_1");


        var usersActual = _connectionRepository.GetAll().Select(kv => kv.Value.User);
        var connectionsActual = _connectionRepository.GetAll().ToArray();
        var user1ConnectionsActual = connectionsActual.First(x => x.Value.User == user1)
            .Value.ConnectionIds;
        var user2ConnectionsActual = connectionsActual.First(x => x.Value.User == user2)
            .Value.ConnectionIds;

        var usersExpected = new [] {user1, user2};
        var user1ConnectionsExpected = new [] { "1_1", "1_3" };
        var user2ConnectionsExpected = new [] { "2_2", "2_3", "2_5" };

        Assert.Multiple(() =>
        {
            Assert.That(usersActual, Is.EquivalentTo(usersExpected));
            Assert.That(user1ConnectionsActual, Is.EquivalentTo(user1ConnectionsExpected));
            Assert.That(user2ConnectionsActual, Is.EquivalentTo(user2ConnectionsExpected));
        });
    }
}