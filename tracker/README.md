# Clicker tracker project



run web mock server

```bash
python3 -m http.server 8081
```

### Tracker.js

Is a simple script that collect `click` & `load` events

```js
{
    "event_type": "page-view",
    "page_url": "http://localhost:8081/",
    "page_title": "App example",
    "path_name": "/",
    "id": "26c5b263-a3e3-4954-b73c-87c780855bca",
    "country": "US",
    "locale": "en-US",
    "platform": "MacIntel",
    "user_agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36 Edg/115.0.1901.183",
    "timestamp": 1690497857809
}

{
    "event_type": "click",
    "element_id": "BUTTON",
    "element_type": "BUTTON",
    "element_text": "click",
    "id": "3d867f89-8006-405a-a9ce-a6f121f931ea",
    "country": "US",
    "locale": "en-US",
    "platform": "MacIntel",
    "user_agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36 Edg/115.0.1901.183",
    "timestamp": 1690497968694
}
```

For the next iteration we collect the time spended in the `page-view`, `select`, and `change` events

