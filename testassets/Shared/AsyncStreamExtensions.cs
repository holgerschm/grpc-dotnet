#region Copyright notice and license

// Copyright 2019 The gRPC Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using Grpc.Core;

namespace Grpc.Shared.TestAssets;

// Implementation copied from https://github.com/grpc/grpc/blob/master/src/csharp/Grpc.Core/Utils/AsyncStreamExtensions.cs
internal static class AsyncStreamExtensions
{
    /// <summary>
    /// Reads the entire stream and executes an async action for each element.
    /// </summary>
    public static async Task ForEachAsync<T>(this IAsyncStreamReader<T> streamReader, Func<T, Task> asyncAction)
        where T : class
    {
        while (await streamReader.MoveNext())
        {
            await asyncAction(streamReader.Current);
        }
    }

    /// <summary>
    /// Reads the entire stream and creates a list containing all the elements read.
    /// </summary>
    public static async Task<List<T>> ToListAsync<T>(this IAsyncStreamReader<T> streamReader)
        where T : class
    {
        var result = new List<T>();
        while (await streamReader.MoveNext())
        {
            result.Add(streamReader.Current);
        }
        return result;
    }

    /// <summary>
    /// Writes all elements from given enumerable to the stream.
    /// Completes the stream afterwards unless close = false.
    /// </summary>
    public static async Task WriteAllAsync<T>(this IClientStreamWriter<T> streamWriter, IEnumerable<T> elements, bool complete = true)
        where T : class
    {
        foreach (var element in elements)
        {
            await streamWriter.WriteAsync(element);
        }
        if (complete)
        {
            await streamWriter.CompleteAsync();
        }
    }

    /// <summary>
    /// Writes all elements from given enumerable to the stream.
    /// </summary>
    public static async Task WriteAllAsync<T>(this IServerStreamWriter<T> streamWriter, IEnumerable<T> elements)
        where T : class
    {
        foreach (var element in elements)
        {
            await streamWriter.WriteAsync(element);
        }
    }
}
