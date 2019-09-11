using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDo.Infrastructure;
using ToDoApi.DTOs;
using Serilog;
using ToDoApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ToDoApi.Services
{
    public class ToDoListsService
    {
        private readonly ToDoDbContext _context;

        public ToDoListsService(ToDoDbContext context)
        {
            _context = context;
        }

        public ToDoList GetToDoList(Guid id, string owner) => _context.ToDoLists
                .Include(x => x.Items)
                .SingleOrDefault(l => l.Owner == owner && l.Id.Equals(id));

        public List<ToDoListDTO> GetToDoLists(string owner) => _context.ToDoLists
                .Include(x => x.Items)
                .Where(x => x.Owner == owner)
                .OrderByDescending(l => l.Position)
                .Select(l => new ToDoListDTO(l)).ToList();

        public void DeleteToDoList(Guid id, string owner)
        {
            ToDoList list = GetToDoList(id, owner);

            if (list == null)
                throw new EntityNotFoundException();
            else if (list.Owner != owner)
                throw new UnauthorizedException();

            foreach (var toDoList in _context.ToDoLists.Where(l => l.Position > list.Position))
                toDoList.Position--;

            _context.ToDoLists.Remove(list);
            _context.SaveChanges();
        }

        public List<ToDoListDTO> SearchToDoLists(string title, string owner) => GetToDoLists(owner).
            Where(l => l.Title != null && l.Title.ToLower().Contains(title.ToLower())).ToList();

        public ToDoList CreateToDoList(ToDoListDTO list, string owner)
        {
            ToDoList result = list.ToEntity();
            result.Position = _context.ToDoLists.Where(l => l.Owner == owner).Count();
            result.Reminded = true;
            result.Owner = owner;
            _context.ToDoLists.Add(result);
            _context.SaveChanges();

            return result;
        }

        public void UpdateToDoList(ToDoListDTO list, string owner)
        {
            ToDoList oldList = GetToDoList(list.Id, owner);

            if (oldList == null)
                throw new EntityNotFoundException();
            else if (oldList.Owner != owner)
                throw new UnauthorizedException();

            oldList.Update(list.ToEntity());
            _context.SaveChanges();
        }

        public ToDoItemDTO GetToDoItem(Guid listId, Guid itemId, string owner)
        {
            ToDoList toDoList = GetToDoList(listId, owner);

            if (toDoList == null)
                throw new EntityNotFoundException();
            else if (toDoList.Owner != owner)
                throw new UnauthorizedException();

            return new ToDoItemDTO(toDoList.Items.SingleOrDefault(i => i.Id.Equals(itemId)));
        }

        public List<ToDoItemDTO> GetToDoItems(Guid listId, string owner)
        {
            ToDoList toDoList = GetToDoList(listId, owner);

            if (toDoList == null)
                throw new EntityNotFoundException();
            else if (toDoList.Owner != owner)
                throw new UnauthorizedException();

            return toDoList.Items
                .Select(i => new ToDoItemDTO(i))
                .OrderBy(i => i.Position).ToList();
        }

        public void DeleteToDoItem(Guid listId, Guid itemId, string owner)
        {
            ToDoList toDoList = GetToDoList(listId, owner);

            if (toDoList == null)
                throw new EntityNotFoundException();
            else if (toDoList.Owner != owner)
                throw new UnauthorizedException();

            ToDoItem item = toDoList.Items.SingleOrDefault(i => i.Id.Equals(itemId));

            if (item == null)
                throw new EntityNotFoundException();

            foreach (var i in toDoList.Items.Where(it => it.Position > item.Position))
                i.Position--;

            toDoList.Items.Remove(item);
            _context.SaveChanges();
        }

        public ToDoItem CreateToDoItem(Guid listId, ToDoItemDTO item, string owner)
        {
            ToDoList toDoList = GetToDoList(listId, owner);

            if (toDoList == null)
                throw new EntityNotFoundException();
            else if (toDoList.Owner != owner)
                throw new UnauthorizedException();

            ToDoItem modelItem = item.ToEntity(toDoList);
            modelItem.Position = toDoList.Items.Count;
            toDoList.Items.Add(modelItem);
            _context.SaveChanges();

            return modelItem;
        }

        public void UpdateToDoItem(Guid listId, ToDoItemDTO item, string owner)
        {
            ToDoList toDoList = GetToDoList(listId, owner);

            if (toDoList == null)
                throw new EntityNotFoundException();
            else if (toDoList.Owner != owner)
                throw new UnauthorizedException();

            ToDoItem oldItem = toDoList.Items.SingleOrDefault(i => i.Id.Equals(item.Id));
            if (oldItem == null)
                throw new EntityNotFoundException();

            oldItem.Update(item.ToEntity(toDoList));
            _context.SaveChanges();
        }

        public List<ToDoListDTO> UpdateToDoListPosition(Guid listId, int position, string owner)
        {
            ToDoList toDoList = GetToDoList(listId, owner);

            if (toDoList == null)
                throw new EntityNotFoundException();

            int maxPosition = _context.ToDoLists.Where(l => l.Owner == owner).Count() - 1;

            if (position > maxPosition || position < 0)
                throw new InvalidPositionException();


            List<ToDoList> requiresUpdate;
            if (position > toDoList.Position)
            {
                requiresUpdate = _context.ToDoLists.Where(l => l.Owner == owner && l.Position <= position && l.Position > toDoList.Position).ToList();
                foreach (ToDoList list in requiresUpdate)
                {
                    list.Position--;
                }
            }
            else if (position < toDoList.Position)
            {
                requiresUpdate = _context.ToDoLists.Where(l => l.Owner == owner && l.Position >= position && l.Position < toDoList.Position).ToList();
                foreach (ToDoList list in requiresUpdate)
                {
                    list.Position++;
                }
            }
            toDoList.Position = position;
            _context.SaveChanges();

            return GetToDoLists(owner);
        }

        public List<ToDoItemDTO> UpdateToDoItemPosition(Guid listId, Guid itemId, int position, string owner)
        {
            ToDoList toDoList = GetToDoList(listId, owner);

            if (toDoList == null)
                throw new EntityNotFoundException();
            else if (toDoList.Owner != owner)
                throw new UnauthorizedException();

            ToDoItem toDoItem = toDoList.Items.SingleOrDefault(i => i.Id.Equals(itemId));

            if (toDoItem == null)
                throw new EntityNotFoundException();

            int maxPosition = toDoList.Items.Count - 1;

            if (position > maxPosition || position < 0)
                throw new InvalidPositionException();

            toDoItem.UpdatePosition(position);

            _context.SaveChanges();

            return GetToDoItems(listId, owner);
        }

        public ToDoListShare CreateToDoListShare(Guid listId, string owner)
        {
            ToDoList toDoList = GetToDoList(listId, owner);

            if (toDoList == null)
                throw new EntityNotFoundException();
            else if (toDoList.Owner != owner)
                throw new UnauthorizedException();

            ToDoListShare share = new ToDoListShare(toDoList);
            _context.ToDoListShares.Add(share);
            _context.SaveChanges();

            return share;
        }

        public ToDoListDTO GetToDoListShare(Guid shareId)
        {
            ToDoListShare share = _context.ToDoListShares.Include(s => s.ToDoList)
                .ThenInclude(l => l.Items)
                .Where(s => s.Id == shareId && DateTime.Compare(s.ExpiresOn, DateTime.Now) > 0)
                .SingleOrDefault();

            if (share == null)
                throw new EntityNotFoundException();

            return new ToDoListDTO(share.ToDoList);
        }
    }
}
