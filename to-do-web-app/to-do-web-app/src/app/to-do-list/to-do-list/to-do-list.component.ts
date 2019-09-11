import { Component, OnInit, OnDestroy } from '@angular/core';
import { ToDoList } from 'src/app/models/to-do-list.model';
import { ActivatedRoute } from '@angular/router';
import { ToDoApiService } from 'src/app/core/to-do-api.service';
import { ToDoItem } from 'src/app/models/to-do-item.model';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { ToDoListService } from '../to-do-list.service';
import * as moment from 'moment';

@Component({
  selector: 'app-to-do-list',
  templateUrl: './to-do-list.component.html',
  styleUrls: ['./to-do-list.component.css']
})
export class ToDoListComponent implements OnInit, OnDestroy {
  toDoList: ToDoList = new ToDoList();
  sub: any;
  toastrService: ToastrService;
  apiService: ToDoApiService;
  subscription: Subscription;

  constructor(private route: ActivatedRoute, apiService: ToDoApiService, toastrService: ToastrService, private toDoListService: ToDoListService) {
    this.apiService = apiService;
    this.toastrService = toastrService;

    this.subscription = this.toDoListService.getToDoList.subscribe(message => {
      if (message) {
        this.toDoList.id = message.id;
      }
    });
  }

  completed(): ToDoItem[] {
    return this.toDoList.items.filter(i => i.completed);
  }

  notCompleted(): ToDoItem[] {
    return this.toDoList.items.filter(i => !i.completed);
  }

  editOrCreateToDoList() {
    if (this.toDoList.id != null) {
      this.editToDolist();
    } else {
      this.createToDoList();
    }
  }

  editToDolist() {
    this.apiService.editToDoList(this.toDoList).subscribe({
      error: () => {
        this.toastrService.error("Failed to edit list");
        this.getToDoList(this.toDoList.id);
      }
    })
  }

  createToDoList() {
    this.apiService.createToDoList(this.toDoList).subscribe({
      next: result => {
        this.toDoList = result;
      }
    })
  }

  getToDoList(id: string) {
    this.apiService.getToDoList(id).subscribe(
      {
        next: result => {
          this.toDoList = result;
          if (!this.toDoList.reminded) {
            this.toDoList.reminded = true;
            this.editToDolist();
          }
        }
      }
    );
  }

  onRefreshItems() {
    this.getToDoItems();
  }

  getToDoItems() {
    this.apiService.getToDoItems(this.toDoList.id).subscribe({
      next: result => {
        this.toDoList.items = result;
      }
    });
  }

  addReminder() {
    if (!this.toDoList.reminded || !moment(this.toDoList.reminderDate).isValid()) {
      this.toastrService.error("Invalid reminder date format");
      return;
    } else if (moment(this.toDoList.reminderDate).isBefore(moment())) {
      this.toastrService.error("Reminder date cannot be in past");
      return;
    }
    this.toDoList.reminded = false;
    this.editToDolist();
  }

  onMove(toDoItem: ToDoItem, i: number) {
    this.apiService.updateToDoItemPosition(this.toDoList.id, toDoItem.id, i).subscribe({
      next: (data: ToDoItem[]) => {
        this.toDoList.items = data;
      },
      error: () => {
        this.toastrService.error("Failed to change item position");
        this.getToDoItems();
      }
    })
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      if (params['id'])
        this.getToDoList(params['id']);
    })
    moment.locale();
  }

  ngOnDestroy() {
    this.apiService.editToDoList(this.toDoList);
    this.subscription.unsubscribe();
  }
}
