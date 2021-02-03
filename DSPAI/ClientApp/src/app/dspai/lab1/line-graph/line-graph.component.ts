import { Component, OnInit, AfterViewInit, OnChanges } from '@angular/core';
import { ViewChild } from '@angular/core';

@Component({
  selector: 'app-line-graph',
  templateUrl: './line-graph.component.html',
  styleUrls: ['./line-graph.component.css']
})
export class LineGraphComponent implements OnInit {

  @ViewChild('cchart') cchart;

  histogramData = {
    chartType: 'LineChart',
    dataTable: [["1","Histogramma"],[0,0]],
    options:{
      title: 'Company Performance',
      curveType: 'function',
      legend: { position: 'bottom' }
    }
  };

  condition: boolean=false;

  constructor() { 
    console.log('constructor');
    console.log(this.cchart)
  }

  ngOnInit() {
    console.log('ngOnInti');
    console.log(this.cchart)
  }

  ngOnChanges(){
    console.log("cc")
  }
  
  ngAfterViewInit(){
    console.log('123');
    console.log(this.cchart)
  } 

  createHistograma(res){
    let dataTable = this.cchart.wrapper.getDataTable();
    let length = dataTable.hc.length;
    if(length < 256){
      dataTable.addRows(256 - length);
    }
    for(let i = 1; i < 256; i++){
      dataTable.setValue(i, 0, String(i - 1));
      dataTable.setValue(i, 1, res[i - 1]);
    }  
    this.cchart.redraw();
  }
}
