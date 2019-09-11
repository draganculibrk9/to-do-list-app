import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ToDoList } from 'src/app/models/to-do-list.model';
import { ToDoApiService } from 'src/app/core/to-do-api.service';
import { faBell, faShare } from '@fortawesome/free-solid-svg-icons';
import { library } from '@fortawesome/fontawesome-svg-core';
import { ClipboardService } from 'ngx-clipboard';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'to-do-preview',
  templateUrl: './to-do-preview.component.html',
  styleUrls: ['./to-do-preview.component.css']
})
  export class ToDoPreviewComponent implements OnInit {
  @Input() toDoList: ToDoList;
  @Output() removeToDoListEvent: EventEmitter<string> = new EventEmitter();
  api: ToDoApiService;

  constructor(api: ToDoApiService, private clipboardService: ClipboardService, private toastrService: ToastrService) { 
    this.api = api;
    library.add(faBell, faShare);
  }

  emitRemoveToDoList() {
    this.api.deleteToDoList(this.toDoList.id).subscribe({
      next: () => {
        this.removeToDoListEvent.emit(this.toDoList.id);
      }
    });
  }

  createToDoListShare() {
    this.api.createToDoListShare(this.toDoList.id).subscribe({
      next: result => {
        this.clipboardService.copyFromContent(`localhost:4200/to-do-list-shares/${result}`);
        this.toastrService.success("Link copied to clipboard");
      },
      error: (err) => {
        this.toastrService.error("Failed to create share link");
      }
    })
  }

  ngOnInit() {
  }

}
