import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { DashboardModule } from './dashboard/dashboard.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { CoreModule } from './core/core.module';
import { ToDoListModule } from './to-do-list/to-do-list.module';
import { DndModule } from 'ng2-dnd';
import { SearchService } from './search.service';
import { FormsModule } from '@angular/forms';
import { ToDoListShareModule } from './to-do-list-share/to-do-list-share.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    DashboardModule,
    CommonModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-center'
    }),
    CoreModule,
    ToDoListModule,
    DndModule.forRoot(),
    FormsModule,
    ToDoListShareModule
  ],
  providers: [
    SearchService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
