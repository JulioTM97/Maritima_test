import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { MasterCompanyEmployees } from './components/MasterCompanyEmployees/MasterCompanyEmployees.component';

@NgModule({
  declarations: [
    AppComponent,
    MasterCompanyEmployees
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
