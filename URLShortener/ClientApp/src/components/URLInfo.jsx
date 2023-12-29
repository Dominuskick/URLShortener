import React, { useState, useEffect, Component, useRef } from 'react';

export function URLInfo() {
  const apiURLInfo = "/urlinfo/";
  const [urlInfos, setUrlInfos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [newUrl, setNewUrl] = useState("");
  let counter = 0;
  
  useEffect(() => {
    populateURLInfoData();
  }, []);

  const renderURLInfoTable = (urlInfos) => {
    return (
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>fullURL</th>
            <th>shortenURL</th>
            <th>createdBy</th>
            <th>createdDate</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {urlInfos.map(urlInfo => (
            <tr key={urlInfo.urlId}>
              <td>{urlInfo.fullURL}</td>
              <td>{urlInfo.shortenURL}</td>
              <td>{urlInfo.createdBy}</td>
              <td>{urlInfo.createdDate}</td>
              <td>
                <button onClick={() => deleteURLInfo(urlInfo.urlId)}>
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  }


  const populateURLInfoData = async() => {
    //const token = 'FCABFDBE-8A18-4F4A-2189-08DC0803AEAB';

    const options = {
        method: "GET",
        //headers: {
        //    "Authorization": `Bearer ${token}`,
        //    "Content-Type": "application/json"
       // }
    }

    const response = await fetch(apiURLInfo, options);
        if(response.ok){
        const data = await response.json();
        setUrlInfos(data);
        setLoading(false);
    }
    else{
        
    }
  }

  const updateURLInfo = async(urlInfo) => {
    const options = {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(urlInfo.url),
    };
  
    const response = await fetch(`${apiURLInfo}${urlInfo.urlId}`, options);
    if (response.ok) {
      const updatedData = await response.json();
      this.setState((prevState) => ({
        urlInfos: prevState.urlInfos.map((item) =>
          item.urlId === updatedData.urlId ? updatedData : item
        ),
      }));
    } else {
      // Handle error
    }
  }
    
  const deleteURLInfo = async(urlId) => {
    const options = {
      method: "DELETE",
    };
  
    const response = await fetch(`${apiURLInfo}/${urlId}`, options);
    if (response.ok) {
      setUrlInfos(prevUrlInfos =>
        prevUrlInfos.filter((item) => item.urlId !== urlId)
      );
    } else {
      // Handle error
    }
  }


  const createURLInfo = async() => {
    const options = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(newUrl),
    };
  
    const response = await fetch(`${apiURLInfo}`, options);
    if (response.ok) {
      const newData = await response.text();
      setUrlInfos(prevUrlInfos => [...prevUrlInfos, newData]);
      setNewUrl("");
    } else {
      console.error("Error:", response.statusText);
      // Handle error
    }
  }
  
  
  const contents = loading ? (
    <p>
      <em>Loading...</em>
    </p>
  ) : (
    renderURLInfoTable(urlInfos)
  );
  
  return (
    <div>
      <h1 id="tableLabel2">URLs info</h1>
      {contents}
      <input
        type="text"
        value={newUrl}
        onChange={(e) => setNewUrl(e.target.value)}
        placeholder="Enter new URL"
      />
      <button onClick={createURLInfo}>Create URL</button>
    </div>
  );
  
}
