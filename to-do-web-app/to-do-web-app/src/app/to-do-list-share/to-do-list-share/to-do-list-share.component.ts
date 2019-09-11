import { Component, OnInit } from '@angular/core';
import { ToDoList } from 'src/app/models/to-do-list.model';
import { ToDoApiService } from 'src/app/core/to-do-api.service';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ToDoItem } from 'src/app/models/to-do-item.model';

@Component({
  selector: 'app-to-do-list-share',
  templateUrl: './to-do-list-share.component.html',
  styleUrls: ['./to-do-list-share.component.css']
})
export class ToDoListShareComponent implements OnInit {
  toDoList: ToDoList = new ToDoList();
  apiService: ToDoApiService;
  toastrService: ToastrService;

  constructor(private route: ActivatedRoute, apiService: ToDoApiService, toastrService: ToastrService) {
    this.apiService = apiService;
    this.toastrService = toastrService;
  }

  ngOnInit() {
    this.getToDoListShare(this.route.snapshot.paramMap.get("id"));
  }

  getToDoListShare(shareId: string) {
    this.apiService.getToDoListShare(shareId).subscribe(
      {
        next: result => {
          this.toDoList = result;
        },
        error: () => {
          this.toastrService.error("Share link is invalid or expired");
        }
      }
    )
  }

  completed(): ToDoItem[] {
    return this.toDoList.items.filter(i => i.completed);
  }

  notCompleted(): ToDoItem[] {
    return this.toDoList.items.filter(i => !i.completed);
  }
}
