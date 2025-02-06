import { Component } from '@angular/core';
import { HttpClient, HttpClientModule, HttpParams } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss'],
  standalone: true,
  imports: [
    FormsModule,
    HttpClientModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatFormFieldModule,
    MatButtonModule
  ]
})
export class MainPageComponent {
  selectedProcessor: string | null = null;
  file: File | null = null;
  fileName: string = '';
  outputFileName: string = '';

  constructor(private http: HttpClient) {}

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      this.file = input.files[0];
      this.fileName = this.file.name;
    } else {
      this.file = null;
      this.fileName = '';
    }
  }

  onSubmit() {
    if (this.file && this.selectedProcessor && this.outputFileName) {
      const params = new HttpParams()
        .set('outputFileName', this.outputFileName)
        .set('processType', this.selectedProcessor);

      const url = `http://localhost:5144/file/process`;

      const formData = new FormData();
      formData.append('file', this.file);

      this.http.post(url, formData, { params: params, responseType: 'blob' })
        .subscribe(response => {
          this.downloadFile(response, `${this.outputFileName}`);
        }, error => {
          console.error('Ошибка при обработке файла:', error);
        });
    }
  }

  private downloadFile(data: Blob, fileName: string) {
    const url = window.URL.createObjectURL(data);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
  }
}