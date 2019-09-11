import { Component, OnInit, OnDestroy } from '@angular/core';
import { ToDoApiService } from '../core/to-do-api.service';
import { ToDoList } from '../models/to-do-list.model';
import { ToastrService } from 'ngx-toastr';
import { SearchService } from '../search.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
  apiService: ToDoApiService;
  toDoLists: ToDoList[] = [];
  toastrService: ToastrService;
  subscription: Subscription;

  constructor(apiService: ToDoApiService, toastrService: ToastrService, private searchService: SearchService) {
    this.apiService = apiService;
    this.toastrService = toastrService;

    this.subscription = this.searchService.getSearchResults().subscribe(lists => {
      if(lists) {
        this.toDoLists = lists;
      }
    })
  }

  getToDoLists() {
    this.apiService.getToDoLists().subscribe({
      next: result => {
        this.toDoLists = result;
      }
    }
    );
  }

  reminded(): ToDoList[] {
    return this.toDoLists.filter(l => l.reminded);
  }

  notReminded(): ToDoList[] {
    return this.toDoLists.filter(l => !l.reminded);
  }

  onMove(toDoList: ToDoList, i: number) {
    this.apiService.updateToDoListPosition(toDoList.id, i).subscribe({
      next: (data: ToDoList[]) => {
        this.toDoLists = data;
      },
      error: () => {
        this.toastrService.error("Failed to change list position");
        this.getToDoLists();
      }
    })
  }

  ngOnInit() {
    this.getToDoLists();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
