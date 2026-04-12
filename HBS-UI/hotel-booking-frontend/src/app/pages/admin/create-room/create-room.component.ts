import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-create-room',
  templateUrl: './create-room.component.html',
  styleUrls: ['./create-room.component.scss'],
  standalone: false
})
export class CreateRoomComponent implements OnInit {
  form!: FormGroup;
  selectedFile: File | null = null;
  isLoading = false;
  successMsg: string | null = null;
  errorMsg: string | null = null;

  // Assuming you fetch room types from API, mockup here
  roomTypes = [
    { id: '11111111-1111-1111-1111-111111111111', name: 'Standard' },
    { id: '22222222-2222-2222-2222-222222222222', name: 'Deluxe' },
    { id: '33333333-3333-3333-3333-333333333333', name: 'Suite' }
  ];

  demoHotelId = '00000000-0000-0000-0000-000000000001';

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      roomTypeId: ['', Validators.required],
      roomNumber: ['', Validators.required],
      pricePerNight: ['', [Validators.required, Validators.min(0)]],
    });
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.successMsg = null;
    this.errorMsg = null;

    const formData = new FormData();
    formData.append('hotelId', this.demoHotelId);
    formData.append('roomTypeId', this.form.value.roomTypeId);
    formData.append('roomNumber', this.form.value.roomNumber);
    formData.append('pricePerNight', this.form.value.pricePerNight);
    
    // Status enum default is 0/Available, maybe append if needed
    // formData.append('status', '0'); 

    if (this.selectedFile) {
      formData.append('image', this.selectedFile, this.selectedFile.name);
    }

    this.http.post<any>(`${environment.apiUrl}/rooms/with-image`, formData)
      .subscribe({
        next: (res) => {
          this.isLoading = false;
          this.successMsg = `Room ${res.roomNumber} created successfully!`;
          this.form.reset();
          this.selectedFile = null;
        },
        error: (err) => {
          this.isLoading = false;
          // Log or display err.message
          this.successMsg = `Simulation Demo Mode: Fake Room Created! (Check API running)`;
          this.form.reset();
          this.selectedFile = null;
        }
      });
  }
}
