import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { toDoItems: [], loading: true };
  }

  componentDidMount() {
    this.populateToDoItems();
  }

  static renderToDoItemsTable(toDoItems) {
    return (
      <table className='table table-striped' aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Completed</th>
          </tr>
        </thead>
        <tbody>
          {toDoItems.map(toDoItem =>
            
            <tr key={toDoItem.id}>
              <td>{toDoItem.id}</td>
              <td>{toDoItem.name}</td>
              <td>{String(toDoItem.isComplete)}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderToDoItemsTable(this.state.toDoItems);

    return (
      <div>
        <h1 id="tableLabel" >To Do Items</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateToDoItems() {
    const response = await fetch('https://localhost:10443/api/todoitems');
    const data = await response.json();
    this.setState({ toDoItems: data, loading: false });
  }
}
