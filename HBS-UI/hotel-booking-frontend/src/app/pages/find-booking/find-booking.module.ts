import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { FindBookingComponent } from './find-booking.component';

const routes: Routes = [{ path: '', component: FindBookingComponent }];

@NgModule({
  declarations: [FindBookingComponent],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(routes)
  ]
})
export class FindBookingModule { }
