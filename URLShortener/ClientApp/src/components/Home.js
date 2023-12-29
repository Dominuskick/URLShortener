import React, { Component } from 'react';
import UserAddModal from './modals/UserAddModal';

export class Home extends Component {
  static displayName = Home.name;
  constructor(props) {
    super(props);
    
    this.userAddModal = React.createRef();
  }

  render() {
    return (
      <div>
        <h1>URL Shortener</h1>
        <p>doesn`t work authorization but at least i try</p>
        <div>
          <button onClick={this.handleRegisterClick}>Register</button>
          <button onClick={this.handleLoginClick}>Login</button>
        </div>

        <UserAddModal 
                    ref={this.userAddModal}
                    onSuccess={this.populateURLInfoData} />
      </div>
    );
  }
}
