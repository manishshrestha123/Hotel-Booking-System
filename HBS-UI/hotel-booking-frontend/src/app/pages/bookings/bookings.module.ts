import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BookingsRoutingModule } from './bookings-routing.module';
import { BookingsComponent } from './bookings.component';
import { CreateBookingComponent } from './create-booking/create-booking.component';

@NgModule({
  declarations: [BookingsComponent, CreateBookingComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule, BookingsRoutingModule]
})
export class BookingsModule {}
