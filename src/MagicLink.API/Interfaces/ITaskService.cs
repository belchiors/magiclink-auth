using MagicLink.API.Models;

namespace MagicLink.API.Interfaces;

public interface ITaskService
{
    void SendMessage(byte[] message);
}