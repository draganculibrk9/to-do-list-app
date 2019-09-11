import { Component, OnInit } from '@angular/core';
import { SearchService } from './search.service';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'to-do-web-app';

  constructor(private searchService: SearchService, public authService: AuthService) {}

  search(title: string) {
    this.searchService.search(title);
  }

  ngOnInit(): void {
    this.authService.handleLoginCallback();
  }
}
