import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LineGraphComponent } from '../line-graph/line-graph.component';


@Component({
  selector: 'app-first-step-lab1',
  templateUrl: './first-step-lab1.component.html',
  styleUrls: ['./first-step-lab1.component.css']
})
export class FirstStepLab1Component implements OnInit {
  selectedFile: File = null;
  public hist: HistogramDTO;

  constructor(private http: HttpClient) { }

  @ViewChild(LineGraphComponent)
  private graphDrower: LineGraphComponent;

  ngOnInit() {

  }

  url = '';
  onFileSelected(event) {
    this.selectedFile = <File>event.target.files[0];
    var reader = new FileReader();

    reader.readAsDataURL(event.target.files[0]); // read file as data url

    reader.onload = (event) => { // called once readAsDataURL is completed
      this.url = event.target.result;
    }
  }

  onUpload() {
    console.log("1");
    const fd = new FormData();
    fd.append('file', this.selectedFile, this.selectedFile.name);
    this.graphDrower.condition = true;
    this.http.post('http://localhost:54930/api/UploadFile', fd)
      .subscribe(res => {
        this.hist = <HistogramDTO>res;
        console.log("1.5");
        console.log("beforeCreate");
        console.log(this.hist);
        this.graphDrower.createHistograma(this.hist.histogram);

      });
    this.graphDrower.createHistograma([0, 0]);
  }
}

class HistogramDTO {
  histogram: Array<any>;
  path: string;
}